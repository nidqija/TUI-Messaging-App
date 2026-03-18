using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace TUI_Messaging_App.TUI_Messaging_App.Router
    {
        internal class RouterEngine
        {
            // Using Func<string> because every view must return the name of the NEXT route
            private readonly Dictionary<string, Func<string>> routes = new();

            // registers a route with its corresponding action
            public void RegisterRoute(string routeName, Func<string> action)
            {
                string key = routeName.ToLower();
                if (!routes.ContainsKey(key))
                {
                    routes.Add(key, action);
                }
                else
                {
                    throw new ArgumentException($"Route '{routeName}' is already registered.");
                }
            }

            // starts the routing process from a given route
            public void Run(string startRoute)
            {
                string nextRoute = startRoute.ToLower();

                while (nextRoute != "exit")
                {
                    if (routes.ContainsKey(nextRoute))
                    {
                      
                        nextRoute = routes[nextRoute].Invoke().ToLower();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[red]Route '{nextRoute}' not found![/]");
                        nextRoute = "exit";
                    }
                }
            }
        }
    }

