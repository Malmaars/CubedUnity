using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class ChooseService : Node
    {
        Character character;
        List<Service> foundServices = new List<Service>();
        List<Need> needsByPriority = new List<Need>();

        public ChooseService(Character c) { character = c; needsByPriority = character.myNeeds.ToList(); }
        public override Result Run()
        {
            UpdateTracking(character);
            //for now we'll just pick the first service that can be provided by the current room the character is in
            //character.sm.InvokeService(character.currentRoom.GetService(character, 0).service);

            //collect all the possible services for the wanted need
            foundServices.Clear();
            foundServices = FindServices.FindAllServices(character);

            //couldn't find any services (nearly impossible)
            if (foundServices.Count == 0)
            {
                return Result.failed;
            }

            //first sort the needs of the character by priority
            for (int i = 0; i < needsByPriority.Count - 1; i++)
            {
                if (needsByPriority[i].CalculatePriority() < needsByPriority[i + 1].CalculatePriority())
                {
                    Need temp = needsByPriority[i + 1];
                    needsByPriority[i + 1] = needsByPriority[i];
                    needsByPriority[i] = temp;
                    i = -1;
                }
            }

            NeedType serviceType = needsByPriority[0].type;

            bool found = false;
            for (int i = 0; i < needsByPriority.Count; i++)
            {
                serviceType = needsByPriority[i].type;
                foreach (Service s in foundServices)
                {
                    if (s.serviceType == serviceType)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }

            //if found is false, there isn't a service that aligns with the characters needs
            if (!found)
            {
                Debug.Log("No compatible services");
                return Result.failed;
            }

            for (int i = 0; i < foundServices.Count; i++)
            {
                if (foundServices[i].serviceType != serviceType)
                {
                    foundServices.Remove(foundServices[i]);
                    i--;
                }
            }

            //for now all services provide an equal amount of value, so pick one at random
            Service chosenService = foundServices[Random.Range(0, foundServices.Count)];
            chosenService.parent.SetAsker(character);

            character.sm.InvokeService(chosenService, character);

            return Result.success;
        }
    }
}
