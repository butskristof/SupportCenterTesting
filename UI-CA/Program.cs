using System;
using System.Linq;
using System.Reflection;
using Autofac;
using SC.BL;
using SC.DAL;

namespace SC.UI.CA
{
	public class Program
	{
		private static IContainer Container;
		private static Type[] interfaces =
		{
			typeof(ITicketRepository),
			typeof(ITicketManager),
			typeof(ISupportCenterService)
		};
		
		static void Main(string[] args)
		{
			var builder = Setup(); // get interface implementations to use from user
			builder.RegisterType<SupportCenterCA>();
			Container = builder.Build();
			
			RunSupportCenter();
		}

		private static ContainerBuilder Setup()
		{
			// For each defined interface, we'll loop through the possible implementations (which are dynamically
			// gathered) and offer the user a choice for which one has to be used.

			Console.WriteLine("Before starting the console application, we'll configure which implementations are to be used.");
			var builder = new ContainerBuilder();

			foreach (Type iface in interfaces)
			{
				// loop until user passes in a valid option
				bool valid = false;
				while (!valid)
				{
					Console.WriteLine("=================");
					Console.WriteLine(iface.Name);
					Console.WriteLine("=================");

					// get types implementing the current interface
					var types = Assembly.GetAssembly(iface) // access the assembly where the interface is defined
						.GetTypes()
						.Where(t => t.IsClass && t.GetInterfaces().Contains(iface))
						.ToList();
					
					// list options for user
					for (int i = 0; i < types.Count(); ++i)
					{
						Console.WriteLine("\t{0}: {1}", i + 1, types[i].Name);
					}

					Console.Write("Your choice: ");
					int input;
					if (
						Int32.TryParse(Console.ReadLine(), out input) // returns false if non-numeric
						&& input > 0 // these won't be reached if TryParse fails, so they're safe to use
						&& input <= types.Count
					)
					{
						valid = true;
						var type = types[input - 1];
						// register with DI container
						Console.WriteLine("Chosen implementation: {0}", type.Name);
						builder.RegisterType(type).As(iface);
					}
					else
					{
						Console.WriteLine("Invalid input.");
					}

					Console.WriteLine();
				}
			}

			Console.WriteLine("Choices registered, building application...");

			return builder;
		}

		private static void RunSupportCenter()
		{
			using (var scope = Container.BeginLifetimeScope())
			{
				var supportCenter = scope.Resolve<SupportCenterCA>();
				
				supportCenter.RunSupportCenter();
			}
		}
	}
}