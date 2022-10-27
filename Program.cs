using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PoliceWork
{
    struct vehicle
    {
        public int hour;
        public int minute;
        public int second;
        public string licensePlate;

        public vehicle(int hour, int minute, int second, string licensePlate)
        {
            this.hour = hour;
            this.minute = minute;    //when the vehicle passed through the employees
            this.second = second;
            this.licensePlate = licensePlate;  //the vehicles license plate
        }
    }
    class PoliceWork
    {
        static List<vehicle> vehicles = new List<vehicle>();

        //Read and store the data
        static void Task1()
        {
            StreamReader sr = new StreamReader("jarmu.txt");

            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split();
                int hour = int.Parse(line[0]);
                int minute = int.Parse(line[1]);
                int second = int.Parse(line[2]);
                string licensePlate = line[3];

                vehicle item = new vehicle(hour, minute, second, licensePlate);
                vehicles.Add(item);
            }
        }

        //Calculate how many hours have the employees worked if a working hour starts at 00 minute 00 second and end at 59 minute 59 second.
        static void Task2()
        {
            Console.WriteLine("Task 2");
            int start = 0;
            int finish = 0;
            foreach (vehicle item in vehicles)
            {
                if (item.minute != 00 && item.second != 00)
                {
                    start = item.hour + 1;
                    break;
                }
                else
                {
                    start = item.hour;
                    break;
                }
            }
            foreach (vehicle item in vehicles)
            {
                if (item.minute != 59 && item.second != 59)
                {
                    finish = item.hour - 1;
                }
                else
                {
                    finish = item.hour;
                }
            }
            Console.WriteLine($"They worked for{finish - start + 1} hours");
        }

        //In every hour they stop the first vehicle for a check. Print the stopped vehicles on the consol.
        static void Task3()
        {
            Console.WriteLine("Task 3");
            List<int> hours = new List<int>();
            foreach (vehicle item in vehicles)
            {
                if (!hours.Contains(item.hour))
                    hours.Add(item.hour);
            }
            foreach (int h in hours)
            {
                string licensePlate = "";
                foreach (vehicle item in vehicles)
                {
                    if (item.hour == h)
                    {
                        licensePlate = item.licensePlate;
                        break;
                    }
                }
                Console.WriteLine($"{h} óra: {licensePlate}");
            }
        }

        //First latter of the license plate marks the type of the vehicle
        static void Task4()
        {
            Console.WriteLine("4. Task");
            int B = 0; //Bus
            int K = 0; //Truck
            int M = 0; //Motorbike
            int sz = 0; //Cars

            foreach (vehicle item in vehicles)
            {

                if (item.licensePlate.Substring(0, 1) == "B")
                {
                    B++;
                }
                else if (item.licensePlate.Substring(0, 1) == "K")
                {
                    K++;
                }
                else if (item.licensePlate.Substring(0, 1) == "M")
                {
                    M++;
                }
                else
                {
                    sz++;
                }
            }
            Console.WriteLine($"Number of buses: {B}  ");
            Console.WriteLine($"Number of trucks {K}  ");
            Console.WriteLine($"Number of motorbikes {M}  ");
            Console.WriteLine($"Number of cars {sz}  ");
        }

        //When was the longest time period when nothing has passed the employees
        static void Task5()
        {
            Console.WriteLine("Task 5");
            int startingHour = 0;
            int startingMinute = 0;
            int startingSecond = 0;
            int endingHour = 0;
            int endingMinute = 0;
            int endingSecond = 0;
            int previousVehicle = 0;
            int currentVehicle = 0;
            int PreviousLongestEmptyTimePeriod = 0;
            int h = 0;
            int m = 0;
            int s = 0;
            int first = 0;
            foreach (vehicle item in vehicles)
            {
                first++;
                currentVehicle = item.hour * 60 * 60 + item.minute * 60 + item.second; //when the vehicle passes

                if (currentVehicle - previousVehicle > PreviousLongestEmptyTimePeriod && first != 1)
                {
                    PreviousLongestEmptyTimePeriod = currentVehicle - previousVehicle; //storing the previous longest empty period so whilst iterating through the list we end up comparing all the empty time periods
                    startingHour = h;
                    startingMinute = m;
                    startingSecond = s;

                    endingHour = item.hour;
                    endingMinute = item.minute;
                    endingSecond = item.second;
                }

                h = item.hour;
                m = item.minute;
                s = item.second;
                previousVehicle = currentVehicle; //as we iterate through the list the current becomes the previous
                                                  //and we can calculate the empty time period by deducting them from each other
            }
            Console.WriteLine($"{ startingHour}:{ startingMinute}:{ startingSecond} - {endingHour}:{endingMinute}:{endingSecond}");

        }

        //Let the user type in a license plate that with "*" instead of the latters they can not recall
        //List the license plates fitting that given license plate
        static void Task6()
        {
            Console.WriteLine("Task 6");
            Console.WriteLine("Type in the license plate (form: AA-1234, for example: F*-27**)");
            string licensPlate = Console.ReadLine();

            List<int> unknownIndexes = new List<int>();

            for (int i = 0; i < licensPlate.Length; i++)
            {
                if (licensPlate.Substring(i, 1) == "*")
                    unknownIndexes.Add(i);
            }
            foreach (vehicle item in vehicles)
            {
                bool fits = true;
                for (int i = 0; i < item.licensePlate.Length; i++)
                {
                    if (!unknownIndexes.Contains(i) && item.licensePlate.Substring(i, 1) != licensPlate.Substring(i, 1))
                    {
                        fits = false;
                    }
                }
                if (fits)
                {
                    Console.WriteLine($"{item.licensePlate}");
                }
            }
        }

        //A check lasts for 5 minutes
        //if the employees checked the first vehicle, which other vehicles could they check untill the end of the day?
        //list the answer in the vizsgalat.txt
        static void Task7()
        {
            StreamWriter sw = new StreamWriter("vizsgalat.txt");
            int perviousCheckedCar = 0;
            foreach (vehicle item in vehicles)
            {
                if (item.hour * 60 * 60 + item.minute * 60 + item.second > perviousCheckedCar)
                {

                    sw.WriteLine($"{item.hour.ToString().PadLeft(2, '0')} {item.minute.ToString().PadLeft(2, '0')} {item.second.ToString().PadLeft(2, '0')} {item.licensePlate}");
                    perviousCheckedCar = item.hour * 60 * 60 + (item.minute + 5) * 60 + item.second;
                }
            }

            sw.Flush();
            sw.Close();
        }
        static void Main(string[] args)
        {
            Task1();
            Task2();
            Task3();
            Task4();
            Task5();
            Task6();
            Task7();
            Console.ReadKey();
        }
    }
}
