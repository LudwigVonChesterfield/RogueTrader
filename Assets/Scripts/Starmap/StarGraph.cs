using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TriangleNet;
using TriangleNet.Meshing.Algorithm;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Topology;
using TriangleNet.Voronoi;

public class StarGraph
{
    public List<Star> stars;
    public List<HyperspaceCorridor> corridors;

    public GameObject map_obj;
    public GameObject background;

    static Color[] pos_star_colors = new Color[] {
        Color.red, ColorEx.orange, Color.yellow, ColorEx.brown, Color.white, Color.blue, ColorEx.purple
    };
    static float[] pos_star_sizes = new float[] {1.5f, 1.75f, 2.0f};

    public void Init(GameObject new_map_obj, GameObject new_background)
    {
        map_obj = new_map_obj;
        background = new_background;

        stars = new List<Star>();
        corridors = new List<HyperspaceCorridor>();

        GenMap();
    }

    private Star CreateStar(Vector3 pos)
    {
        Star star = Object.Instantiate(Game.inst.star_prefab, pos, Quaternion.identity).GetComponent<Star>();
        star.SetColor(pos_star_colors[Random.Range(0, pos_star_colors.Length)]);
        star.SetSize(pos_star_sizes[Random.Range(0, pos_star_sizes.Length)]);

        star.transform.position += Game.inst.gameWorld.transform.position;
        star.gameObject.transform.SetParent(map_obj.transform);

        return star;
    }

    public void AddStar(Star star)
    {
        stars.Add(star);
    }

    void GenMap()
    {
        /*
            <settings>
        */
        int starsAm = 10000;

        // What percentage of space is taken up by stars.
        int starDensity = 20;

        // Minimal corridor length relative to maximal star size.
        float minCorridorLengthCoeff = 3.0f;

        // What percentage of space is taken up by corridors.
        int corridorDensity = 60;

        float maxz = 3.0f;
        /*
            </settings>
        */

        float max_star_size = Mathf.Max(pos_star_sizes);
        Debug.Log("Max star size: " + max_star_size);
        float min_corridor_length = max_star_size * minCorridorLengthCoeff;
        float min_corridor_length_squared = Mathf.Pow(min_corridor_length, 2);

        int starsx = Mathf.RoundToInt(Mathf.Sqrt(starsAm));
        int starsy = starsx;

        float width = (starsx * max_star_size) + ((starsx - 1) * min_corridor_length);
        float height = (starsy * max_star_size) + ((starsy - 1) * min_corridor_length);

        background.transform.localScale = new Vector3(width * 0.11f, 1.0f, height * 0.11f);
        background.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(width / 70.0f, height / 70.0f));

        Debug.Log("Dimensions: " + width + ", " + height);

        float minx = width * -0.5f;
        float miny = height * -0.5f;

        float maxx = width * 0.5f;
        float maxy = height * 0.5f;

        float max_offset_x = (width / starsx) - ((min_corridor_length - max_star_size) * 0.5f);
        float max_offset_y = (height / starsy) - ((min_corridor_length - max_star_size) * 0.5f);

        Debug.Log("Offsets: " + max_offset_x + ", " + max_offset_y);

        Rectangle bounds = new Rectangle(minx, miny, width, height);
        TriangleNet.Mesh mesh = (TriangleNet.Mesh) GenericMesher.StructuredMesh(bounds, starsx - 1, starsy - 1);

        List<TriangleNet.Geometry.Vertex> stars = new List<TriangleNet.Geometry.Vertex>();

        List<TriangleNet.Geometry.Vertex> shuffled_verts = new List<TriangleNet.Geometry.Vertex>(mesh.Vertices);
        shuffled_verts.Shuffle();

        int stars_amount = 0;
        foreach(TriangleNet.Geometry.Vertex v in shuffled_verts)
        {
            if(stars_amount > 3 && Random.Range(0, 101) > starDensity)
            {
                continue;
            }
            stars.Add(v);
            stars_amount++;
        }
        GenericMesher gm = new GenericMesher(new SweepLine());
        mesh = (TriangleNet.Mesh) gm.Triangulate(stars);

        StandardVoronoi sv = new StandardVoronoi(mesh);

        Polygon final = new Polygon(sv.Vertices.Count);

        Dictionary<int, TriangleNet.Geometry.Vertex> good_stars = new Dictionary<int, TriangleNet.Geometry.Vertex>();
        Dictionary<int, int> bad2goodstar = new Dictionary<int, int>();
        List<int> outBoundStars = new List<int>();
        foreach(TriangleNet.Topology.DCEL.Vertex v in sv.Vertices)
        {
            if(v.x < minx || v.x > maxx || v.y < miny || v.y > maxy)
            {
                outBoundStars.Add(v.id);
                continue;
            }

            bool invalid = false;
            foreach(TriangleNet.Geometry.Vertex other in good_stars.Values)
            {
                Vector2 v1 = new Vector2((float) v.x, (float) v.y);
                Vector2 v2 = new Vector2((float) other.x, (float) other.y);

                if((v2 - v1).sqrMagnitude < min_corridor_length_squared)
                {
                    invalid = true;

                    bad2goodstar[v.id] = other.id;
                }
            }

            if(invalid)
            {
                continue;
            }

            TriangleNet.Geometry.Vertex new_v = new TriangleNet.Geometry.Vertex(v.x, v.y);
            new_v.id = v.id;
            good_stars[v.id] = new_v;
            final.Add(new_v);
        }

        List<Segment> good_segments = new List<Segment>();
        foreach(Edge e in sv.Edges)
        {
            if(outBoundStars.Contains(e.P0) || outBoundStars.Contains(e.P1))
            {
                continue;
            }

            int P0_id;
            int P1_id;

            if(bad2goodstar.ContainsKey(e.P0))
            {
                P0_id = bad2goodstar[e.P0];
            }
            else
            {
                P0_id = e.P0;
            }

            if(bad2goodstar.ContainsKey(e.P1))
            {
                P1_id = bad2goodstar[e.P1];
            }
            else
            {
                P1_id = e.P1;
            }

            if(P0_id == P1_id)
            {
                continue;
            }

            good_segments.Add(new Segment(good_stars[P0_id], good_stars[P1_id]));
        }

        Dictionary<int, List<int>> connected_stars = new Dictionary<int, List<int>>();
        foreach(Segment s in good_segments)
        {
            if(!connected_stars.ContainsKey(s.P0))
            {
                connected_stars[s.P0] = new List<int>();
            }
            connected_stars[s.P0].Add(s.P1);

            if(!connected_stars.ContainsKey(s.P1))
            {
                connected_stars[s.P1] = new List<int>();
            }
            connected_stars[s.P1].Add(s.P0);
        }

        Debug.Log("Currently edges: " + good_segments.Count);

        List<Segment> temp_segments = new List<Segment>(good_segments);
        temp_segments.Shuffle();

        foreach(Segment s in temp_segments)
        {
            if(Random.Range(0, 101) > corridorDensity && RemovalConnectionsCheck(connected_stars, s.P0, s.P1))
            {
                connected_stars[s.P0].Remove(s.P1);
                connected_stars[s.P1].Remove(s.P0);
                good_segments.Remove(s);
            }
        }

        foreach(Segment s in good_segments)
        {
            final.Add(s);
        }

        Debug.Log("Stars after everything: " + final.Points.Count);
        Debug.Log("Corridors after everything: " + good_segments.Count);
        CreateMap(final, maxz, 0.0f);
    }

    bool RemovalConnectionsCheck(Dictionary<int, List<int>> connected_stars, int remove_from, int remove_whom)
    {
        Dictionary<int, List<int>> new_connections = new Dictionary<int, List<int>>();
        foreach(KeyValuePair<int, List<int>> id2stars in connected_stars)
        {
            new_connections[id2stars.Key] = new List<int>(id2stars.Value);
        }
        new_connections[remove_from].Remove(remove_whom);
        new_connections[remove_whom].Remove(remove_from);

        return CheckConnections(remove_from, new_connections, new HashSet<int>(){ remove_from }, true);
    }

    bool CheckConnections(int from_whom, Dictionary<int, List<int>> connected_stars, HashSet<int> passed_through, bool original)
    {
        if(passed_through.Count == connected_stars.Count)
        {
            return true;
        }

        foreach(int star in connected_stars[from_whom])
        {
            if(!passed_through.Contains(star))
            {
                passed_through.Add(star);
                if(CheckConnections(star, connected_stars, passed_through, false))
                {
                    return true;
                }
            }
        }

        return false;
    }

    void CreateMap(List<TriangleNet.Geometry.Vertex> vertices, List<ISegment> segments, float maxz, float yOffset)
    {
        Dictionary<int, Star> id2star = new Dictionary<int, Star>();
        int v_id = 0;
        foreach(TriangleNet.Geometry.Vertex v in vertices)
        {
            Vector3 pos = new Vector3((float) v.x, Random.Range(0.0f, maxz) + yOffset, (float) v.y);
            Star star = CreateStar(pos);

            star.my_text.text = "" + v.id;

            v.id = v_id++;
            id2star[v.id] = star;

            AddStar(star);
        }

        foreach(Segment s in segments)
        {
            Star start = id2star[s.GetVertex(0).id];
            Star end = id2star[s.GetVertex(1).id];

            HyperspaceCorridor corridor = CreateCorridor(start, end);
            AddCorridor(corridor);
        }
    }

    void CreateMap(Polygon poly, float maxz, float yOffset)
    {
        CreateMap(poly.Points, poly.Segments, maxz, yOffset);
    }

    void CreateMap(VoronoiBase sv, float maxz, float yOffset)
    {
        Dictionary<int, TriangleNet.Geometry.Vertex> id2vert = new Dictionary<int, TriangleNet.Geometry.Vertex>();
        List<TriangleNet.Geometry.Vertex> vertices = new List<TriangleNet.Geometry.Vertex>();
        List<ISegment> segments = new List<ISegment>();

        foreach(TriangleNet.Topology.DCEL.Vertex v in sv.Vertices)
        {
            TriangleNet.Geometry.Vertex new_v = new TriangleNet.Geometry.Vertex(v.x, v.y);
            id2vert[v.id] = new_v;
            vertices.Add(new_v);
        }

        foreach(Edge e in sv.Edges)
        {
            TriangleNet.Geometry.Vertex v1 = id2vert[e.P0];
            TriangleNet.Geometry.Vertex v2 = id2vert[e.P1];
            segments.Add(new Segment(v1, v2));
        }
        CreateMap(vertices, segments, maxz, yOffset);
    }

    void CreateMap(TriangleNet.Mesh m, float maxz, float yOffset)
    {
        Dictionary<int, TriangleNet.Geometry.Vertex> id2vert = new Dictionary<int, TriangleNet.Geometry.Vertex>();
        List<TriangleNet.Geometry.Vertex> vertices = new List<TriangleNet.Geometry.Vertex>();
        List<ISegment> segments = new List<ISegment>();

        foreach(TriangleNet.Geometry.Vertex v in m.Vertices)
        {
            id2vert[v.id] = v;
            vertices.Add(v);
        }

        foreach(Edge e in m.Edges)
        {
            TriangleNet.Geometry.Vertex v1 = id2vert[e.P0];
            TriangleNet.Geometry.Vertex v2 = id2vert[e.P1];
            segments.Add(new Segment(v1, v2));
        }
        CreateMap(vertices, segments, maxz, yOffset);
    }

    private HyperspaceCorridor CreateCorridor(Star start, Star end)
    {
        Vector3 pos = (end.pos - start.pos) * 0.5f + start.pos;
        pos += Game.inst.gameWorld.transform.position;

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, end.pos - start.pos);

        HyperspaceCorridor corridor = Object.Instantiate(Game.inst.corridor_prefab, pos, rot).GetComponent<HyperspaceCorridor>();
        corridor.SetStartEnd(start, end);
        corridor.SetColor(Color.yellow);
        // corridor.SetThickness(0.1f);
        corridor.SetCurrentSpeed(Random.Range(0.0f, 1.0f));

        corridor.gameObject.transform.SetParent(map_obj.transform);

        return corridor;
    }

    public void AddCorridor(HyperspaceCorridor corridor)
    {
        corridors.Add(corridor);
    }
}
