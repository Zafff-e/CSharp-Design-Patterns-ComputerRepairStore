using ComputerRepairStoreManagement.Entities;
using ComputerRepairStoreManagement.Factory;
using ComputerRepairStoreManagement.Repositories;
using ComputerRepairStoreManagement.Exceptions;
using ComputerRepairStoreManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerRepairStoreManagement
{
    internal class Program
    {
        const int W = 110;

        static IRepairRepository repo = new RepairRepository();

        static void Main(string[] args)
        {
            Console.Title = "Computer Repair Management System";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try

            {
                DoTask();
            }

            catch (Exception ex)
            {
                Red();
                Console.WriteLine($"\n  [System Error]: {ex.Message}");
                Reset();
            }
            finally
            {
                Console.WriteLine("\n  Press ENTER to exit...");
                Console.ReadLine();
            }
        }

        static void DoTask()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            DrawDoubleLine();
            Cyan();
            PrintCentered("COMPUTER REPAIR MANAGEMENT SYSTEM");
            Reset();
            PrintCentered("Developer: Ahm Zafir Hasan  |  Trainee ID: 1294926  |  Batch ID: CS/SCSL-A/70/01");
            DrawDoubleLine();
            Console.WriteLine();

            while (true)
            {
                MainMenu();
                int input = Convert.ToInt16(Console.ReadLine());
                if (input == 5) break;

                switch (input)
                {
                    case 1: Create(); break;
                    case 2: Read(); break;
                    case 3: Update(); break;
                    case 4: Delete(); break;
                    default:
                        Console.WriteLine("  Invalid Option");
                        break;
                }
            }
        }

        static void MainMenu()
        {
            PrintCentered(new string('~', 83));
            PrintCentered("|| 1. Create     || 2. Read     || 3. Update     || 4. Delete     || 5. Exit     ||");
            PrintCentered(new string('~', 83));
            Console.Write(new string(' ', 13) + "Select Option: ");
        }

        static void Create()
        {
            Console.WriteLine();
            SectionBanner("NEW REPAIR TICKET");

            Console.Write("  Customer Name        : ");
            string name = Console.ReadLine();

            Console.WriteLine("  Device Details       : 1.Laptop, 2.Desktop, 3.Tablet");
            Console.Write("  Choice               : ");
            string dChoice = Console.ReadLine();
            string device = dChoice == "2" ? "Desktop" : (dChoice == "3" ? "Tablet" : "Laptop");

            Console.Write("  Problem Description  : ");
            string desc = Console.ReadLine();

            Console.WriteLine("  Issue Category       : 1.Hardware, 2.Software");
            Console.Write("  Choice               : ");
            RepairCategory category = (RepairCategory)Enum.Parse(typeof(RepairCategory), Console.ReadLine());

            Console.Write("  Advance Token (BDT)  : ");
            double advance = Convert.ToDouble(Console.ReadLine());

            RepairRequest ticket = new RepairRequest(0, name, device, desc, category);
            ticket.AdvancePaid = advance;

            try
            {
                ticket.Validate();
            }
            catch (InvalidRepairRequestException ex)
            {
                Yellow();
                Console.WriteLine($"\n  [Validation Error]: {ex.Message}");
                Reset();
                return;
            }

            RepairFactory factory = null;
            if (category == RepairCategory.Hardware)
            {
                factory = new HardwareRepairFactory(ticket);
                ticket.WarrantyPeriodMonths = 3;
            }
            else if (category == RepairCategory.Software)
            {
                factory = new SoftwareRepairFactory(ticket);
                ticket.WarrantyPeriodMonths = 1;
            }

            if (factory != null)
            {
                factory.ProcessRepair();
            }

            double holidayDisc = HolidayDiscount(ticket);
            double vatAmount = (ticket.TotalEstimate - holidayDisc) * 0.05;
            double finalTotal = (ticket.TotalEstimate - holidayDisc) + vatAmount;

            ticket.TotalEstimate = finalTotal;
            repo.SaveRequest(ticket);

            Read();
            Green();
            Console.WriteLine("\t\t\t\t\t\tRepair ticket saved successfully!");
            Reset();
            Console.WriteLine();
        }

        static void Read()
        {
            Console.WriteLine();
            var tickets = repo.GetAllRequestsList().ToList();

            string fmt = "  | {0,-5} | {1,-16} | {2,-7} | {3,-9} | {4,-13} | {5,-12} | {6,-12} | {7,-11} |";
            string div = "  " + new string('-', 104);

            DrawDoubleLine();
            Cyan();
            PrintCentered("REPAIR TICKET DASHBOARD");
            Reset();
            DrawDoubleLine();

            Cyan();
            Console.WriteLine(fmt, "ID", "Customer Name", "Device", "Category", "Total Cost", "Advance Paid", "Balance Due", "Status");
            Reset();
            Console.WriteLine(div);

            if (!tickets.Any())
            {
                Yellow();
                Console.WriteLine("  No repair tickets on record yet.");
                Reset();
            }
            else
            {
                foreach (var t in tickets)
                {
                    double balance = t.TotalEstimate - t.AdvancePaid;
                    Console.WriteLine(fmt,
                        "#" + t.RequestId,
                        Clip(t.CustomerName, 16),
                        Clip(t.DeviceDetails, 7),
                        t.Category,
                        "BDT " + t.TotalEstimate.ToString("N2"),
                        "BDT " + t.AdvancePaid.ToString("N2"),
                        "BDT " + balance.ToString("N2"),
                        StatusLabel(t.Status));
                }
            }

            Console.WriteLine(div);
            Console.WriteLine();
        }

        static void Update()
        {
            Console.WriteLine();
            Console.Write("  Enter Ticket ID to update: ");
            int id = Convert.ToInt32(Console.ReadLine());

            RepairRequest t2 = repo.GetRequestById(id);
            if (t2 == null)
            {
                Console.WriteLine("  Ticket Not in List");
                return;
            }

            Console.Write($"  Existing Customer Name: {t2.CustomerName}\n  New Name: ");
            t2.CustomerName = Console.ReadLine();

            Console.WriteLine($"  Existing Device Details: {t2.DeviceDetails}\n  New Details: 1.Laptop, 2.Desktop, 3.Tablet");
            Console.Write("  Choice               : ");
            string dChoice2 = Console.ReadLine();
            t2.DeviceDetails = dChoice2 == "2" ? "Desktop" : (dChoice2 == "3" ? "Tablet" : "Laptop");

            Console.Write($"  Existing Description: {t2.ProblemDescription}\n  New Description: ");
            t2.ProblemDescription = Console.ReadLine();

            Console.WriteLine($"  Existing Status: {t2.Status}\n  New Status: 1.Pending, 2.InProgress, 3.Completed");
            Console.Write("  Choice               : ");
            t2.Status = (RepairStatus)Enum.Parse(typeof(RepairStatus), Console.ReadLine());

            Console.Write($"  Existing Advance Paid: {t2.AdvancePaid}\n  New Advance: ");
            t2.AdvancePaid = Convert.ToDouble(Console.ReadLine());

            try
            {
                t2.Validate();
            }
            catch (InvalidRepairRequestException ex)
            {
                Yellow();
                Console.WriteLine($"\n  [Validation Error]: {ex.Message}");
                Reset();
                return;
            }

            RepairFactory factory = (t2.Category == RepairCategory.Hardware)
                ? (RepairFactory)new HardwareRepairFactory(t2)
                : new SoftwareRepairFactory(t2);
            factory.ProcessRepair();

            double holidayDisc = HolidayDiscount(t2);
            double vatAmount = (t2.TotalEstimate - holidayDisc) * 0.05;
            double finalTotal = (t2.TotalEstimate - holidayDisc) + vatAmount;

            t2.TotalEstimate = finalTotal;
            repo.UpdateRequest(t2);

            Read();
            Green();
            Console.WriteLine("\t\t\t\t\t\tTicket updated and costs recalculated successfully!");
            Reset();
            Console.WriteLine();
        }

        static void Delete()
        {
            Console.WriteLine();
            Console.Write("  Enter Ticket ID to remove: ");
            int id = Convert.ToInt32(Console.ReadLine());

            RepairRequest t2 = repo.GetRequestById(id);
            if (t2 == null)
            {
                Console.WriteLine("  Ticket Not in List");
            }
            else
            {
                repo.DeleteRequest(id);
                Read();
                Green();
                Console.WriteLine("\t\t\t\t\t\tTicket removed successfully!");
                Reset();
                Console.WriteLine();
            }
        }


        static double HolidayDiscount(RepairRequest t)
        {
            return PricingCalculator.CalculateHolidayDiscount(t);
        }

        static string StatusLabel(RepairStatus s)
        {
            switch (s)
            {
                case RepairStatus.Pending: return "Pending";
                case RepairStatus.InProgress: return "In Progress";
                case RepairStatus.Completed: return "Handed Over";
                default: return s.ToString();
            }
        }

        static void SectionBanner(string title)
        {
            DrawDoubleLine();
            Cyan();
            PrintCentered(title);
            Reset();
            DrawDoubleLine();
            Console.WriteLine();
        }

        static void DrawDoubleLine() => Console.WriteLine(new string('=', W));
        static void DrawSingleLine() => Console.WriteLine("  " + new string('-', W - 2));

        static void PrintCentered(string text)
        {
            int padding = Math.Max(0, (W - text.Length) / 2);
            Console.WriteLine(new string(' ', padding) + text);
        }

        static void Cyan() => Console.ForegroundColor = ConsoleColor.Cyan;
        static void Green() => Console.ForegroundColor = ConsoleColor.Green;
        static void Yellow() => Console.ForegroundColor = ConsoleColor.Yellow;
        static void Red() => Console.ForegroundColor = ConsoleColor.Red;
        static void Reset() => Console.ResetColor();

        static string Clip(string s, int max) =>
            string.IsNullOrEmpty(s) || s.Length <= max ? s : s.Substring(0, max - 2) + "..";
    }
}
