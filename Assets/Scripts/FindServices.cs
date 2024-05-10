using System.Collections.Generic;
using System;
using UnityEngine;

//TODO: add services from held items
public static class FindServices
{
    static List<Cube> visited = new List<Cube>();
    static List<Service> services = new List<Service>();

    public static List<Service> FindAllServices(Character c)
    {
        services.Clear();
        //return all possible services for the character

        //the first services are the ones that are character specific
        if (c.personalServices != null)
            foreach (Service s in c.personalServices) { services.Add(s); }

        //The other services are from reachable characters and cubes

        //to check this, check each reachable cube, and if there's inhabitants in them
        //first check the current room for services
        if (c.currentRoom.services != null && !c.currentRoom.occupied && !c.currentRoom.beingUsed)
            foreach (Service s in c.currentRoom.services) { services.Add(s); }
        visited.Clear();
        visited.Add(c.currentRoom);

        //depth search for all reachable services
        CheckNeighbours(c.currentRoom);

        //also check inventory items, since these can provide services as well.
        if(c.inventory.Count > 0)
        {
            foreach(InventoryItem it in c.inventory)
            {
                if (it.services != null && it.services.Length > 0)
                {
                    foreach (Service s in it.services)
                    {
                        services.Add(s);
                    }
                }
            }
        }

        return services;
    }

    static void CheckNeighbours(Cube parent)
    {
        foreach (Cube cb in parent.neighbors)
        {
            if (cb == null || visited.Contains(cb))
                continue;

            visited.Add(cb);

            if (cb.occupied)
                continue;

            if (cb.currentInhabitants.Count > 0)
            {
                foreach (Character c in cb.currentInhabitants)
                {
                    if (c.services != null && !c.interacting && !c.beingUsed)
                    foreach (Service s in c.services) { services.Add(s); }
                }
            }

            if (cb.services != null && !cb.occupied && !cb.beingUsed)
                foreach (Service s in cb.services) { services.Add(s); }

            CheckNeighbours(cb);
        }

        return;
    }

    public static List<Service> FindByType(Character c, NeedType _type)
    {
        services.Clear();
        //return all possible services for the character

        //the first services are the ones that are character specific
        if (c.services != null)
            foreach (Service s in c.personalServices) { if(s.serviceType == _type) services.Add(s); }

        //The other services are from reachable characters and cubes

        //to check this, check each reachable cube, and if there's inhabitants in them
        //first check the current room for services

        if (c.currentRoom.services != null && !c.currentRoom.occupied && !c.currentRoom.beingUsed)
            foreach (Service s in c.currentRoom.services) { if (s.serviceType == _type) services.Add(s); }
        
        visited.Clear();
        visited.Add(c.currentRoom);

        //depth search for all reachable services
        CheckNeighboursByType(c.currentRoom, _type);

        //also check inventory items, since these can provide services as well.
        if (c.inventory.Count > 0)
        {
            foreach (InventoryItem it in c.inventory)
            {
                foreach (Service s in it.services)
                {
                    if (s.serviceType == _type)
                        services.Add(s);
                }
            }
        }

        return services;
    }


    static void CheckNeighboursByType(Cube parent, NeedType _type)
    {
        foreach (Cube cb in parent.neighbors)
        {
            if (cb == null || visited.Contains(cb))
                continue;

            visited.Add(cb);

            if (cb.currentInhabitants.Count > 0)
            {
                foreach (Character c in cb.currentInhabitants)
                {
                    if (c.services != null && !c.interacting && !c.beingUsed)
                        foreach (Service s in c.services) { if (s.serviceType == _type) services.Add(s); }
                }
            }

            if (cb.services != null && !cb.occupied && !cb.beingUsed)
                foreach (Service s in cb.services) { if (s.serviceType == _type) services.Add(s); }

            CheckNeighboursByType(parent, _type);
        }

        return;
    }

}
