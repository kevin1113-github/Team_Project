using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    class Room
    {
        public Vector2Int position;
        public Vector2Int size;
        // public bool[] is_door;
        public Vector2Int center;
        public Room(int x, int z, int x_size, int z_size)
        {
            this.position = new Vector2Int(x, z);
            this.size = new Vector2Int(x_size, z_size);
            this.center = new Vector2Int(x+x_size/2, z+z_size/2);
            /*
            this.is_door = new bool[DOOR_COUNT];
            for (int i = 0; i < DOOR_COUNT; i++)
            {
                is_door[i] = (Random.Range(0, 2) == 1);
            }
            */
        }
    }
    const int UP = 0;
    const int RIGHT = 1;
    const int DOWN = 2;
    const int LEFT = 3;

    int[,] map;
    Room[] room_list;
    public int height = 70;
    public int width = 100;
    public int road_size = 1;
    public int room_size = 15;
    public int room_size_range = 5;
    public int room_count = 10;
    public GameObject plane_obj;
    public GameObject wall_obj;
    public GameObject door_obj;
    public GameObject road_obj;

    // const int DOOR_COUNT = 4;
    const int AIR = 0;
    const int WALL = 1;
    const int DOOR = 2;
    const int ROAD = 3;


    // Start is called before the first frame update
    void Start()
    {
        room_list = new Room[room_count];
        map = new int[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[i,j] = AIR;
            }
        }

        int try_count = 0;
        for (int count = 0; count < room_count;)
        {
            int x_size = Random.Range(room_size - room_size_range, room_size + room_size_range);
            int z_size = Random.Range(room_size - room_size_range, room_size + room_size_range);
            int x = Random.Range(0, width - x_size);
            int z = Random.Range(0, height - z_size);
            bool is_collide = false;
            for (int i = 0; i < count; i++)
            {
                if (x < room_list[i].position.x + room_list[i].size.x && x + x_size > room_list[i].position.x &&
                    z < room_list[i].position.y + room_list[i].size.y && z + z_size > room_list[i].position.y)
                {
                    is_collide = true;
                }
            }
            if (!is_collide)
            {
                // Debug.Log("count : " + count);
                room_list[count] = new Room(x, z, x_size, z_size);
                count++;
                try_count = 0;
            }
            else
            {
                try_count++;
                if (try_count > 10)
                {
                    room_count = count;
                    break;
                }
            }
        }

        for (int i = 0; i < room_count; i++)
        {
            int next_room = (i+1 >= room_count ? 0 : i+1);
            Debug.Log(i + ", " + room_count + ", " + next_room);
            bool is_bigger_x_then_next_room = room_list[i].center.x > room_list[next_room].center.x;
            bool is_bigger_y_then_next_room = room_list[i].center.y > room_list[next_room].center.y;
            for (int xV = room_list[i].center.x;
                is_bigger_x_then_next_room ? xV >= room_list[next_room].center.x : xV <= room_list[next_room].center.x;
                xV += is_bigger_x_then_next_room ? -1 : 1)
            {
                map[room_list[i].center.y, xV] = ROAD;
                if (map[room_list[i].center.y + road_size, xV] != ROAD)
                    map[room_list[i].center.y + road_size, xV] = WALL;
                if (map[room_list[i].center.y - road_size, xV] != ROAD)
                    map[room_list[i].center.y - road_size, xV] = WALL;
            }
            for (int zV = room_list[i].center.y;
                is_bigger_y_then_next_room ? zV > room_list[next_room].center.y : zV < room_list[next_room].center.y;
                zV += is_bigger_y_then_next_room ? -1 : 1)
            {
                map[zV, room_list[next_room].center.x] = ROAD;
                if (map[zV, room_list[next_room].center.x + road_size] != ROAD)
                    map[zV, room_list[next_room].center.x + road_size] = WALL;
                if (map[zV, room_list[next_room].center.x - road_size] != ROAD)
                    map[zV, room_list[next_room].center.x - road_size] = WALL;
            }
        }

        for (int i = 0; i < room_count; i++)
        {
            for (int zV = 0; zV < room_list[i].size.y; zV++)
            {
                for (int xV = 0; xV < room_list[i].size.x; xV++)
                {
                    if (zV == 0 || zV == room_list[i].size.y-1 || xV == 0 || xV == room_list[i].size.x-1)
                    {
                        if (map[room_list[i].position.y+zV, room_list[i].position.x+xV] == ROAD)
                            map[room_list[i].position.y+zV, room_list[i].position.x+xV] = DOOR;
                        else
                            map[room_list[i].position.y+zV, room_list[i].position.x+xV] = WALL;
                    }
                    else
                    {

                        map[room_list[i].position.y+zV, room_list[i].position.x+xV] = AIR;
                    }
                }
            }
            /*
            for (int j = 0; j < DOOR_COUNT; j++)
            {
                if (room_list[i].is_door[j])
                {
                    switch (j)
                    {
                        case UP:
                            map[room_list[i].position.y+room_list[i].size.y-1, room_list[i].position.x+room_list[i].size.x/2] = DOOR;
                            break;
                        case RIGHT:
                            map[room_list[i].position.y+room_list[i].size.y/2, room_list[i].position.x+room_list[i].size.x-1] = DOOR;
                            break;
                        case DOWN:
                            map[room_list[i].position.y, room_list[i].position.x+room_list[i].size.x/2] = DOOR;
                            break;
                        case LEFT:
                            map[room_list[i].position.y+room_list[i].size.y/2, room_list[i].position.x] = DOOR;
                            break;
                        default:
                            break;
                    }
                }
            }
            */
        }
        
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                switch (map[i, j])
                {
                    case AIR:
                        Instantiate(plane_obj, new Vector3(j - width/2, 0, i - height/2), Quaternion.identity);
                        break;
                    case WALL:
                        Instantiate(wall_obj, new Vector3(j - width/2, 0, i - height/2), Quaternion.identity);
                        break;
                    case DOOR:
                        Instantiate(door_obj, new Vector3(j - width/2, 0, i - height/2), Quaternion.identity);
                        break;
                    case ROAD:
                        Instantiate(road_obj, new Vector3(j - width/2, 0, i - height/2), Quaternion.identity);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
