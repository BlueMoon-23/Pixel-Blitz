using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapInformation : MonoBehaviour
{
    public Sprite MapImage;
    public string MapName;
    public float StarRate;
    public bool isMultiPath;
    public AudioClip MapBGM;
    [Header("View Bounds")]
    public Tilemap ViewBounds;
    [Header("Placing")]
    public Tilemap PlacingGround;
    public Tilemap[] PlacingCliff;
    public Tilemap PlacingWaypoint;
    public GameObject PlacingGroundUI;
    public GameObject PlacingCliffUI;
    public GameObject WaypointUI;
    [Header("Waypoint")]
    public GameObject WaypointArrows;
    public WaypointInformation[] List_of_Waypoints;
}
