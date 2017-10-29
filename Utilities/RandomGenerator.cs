using System;

namespace Traffic.Utilities
{
    public static class RandomGenerator
    {

        private static Random rnd = new Random();


        /// <summary>
        /// Returns random integer up to max-1
        /// </summary>
        public static int Int(int max)
        {
            return rnd.Next(max);
        }

        /// <summary>
        /// Returns random velocity
        /// </summary>
        public static float Velocity()
        {
            // to preserve that maximum velocity is not lower than initial velocity
            return ((float)rnd.NextDouble()) * (Constants.MaximumVelocity - Constants.InitialVelocity) + Constants.InitialVelocity;
        }

        /// <summary>
        /// Returns random reaction time
        /// </summary>
        public static float ReactionTime()
        {
            return ((float)rnd.NextDouble()) * Constants.MaximumReactionTime;
        }

        /// <summary>
        /// Returns random distance held
        /// </summary>
        public static float DistanceHeld()
        {
            return ((float)rnd.NextDouble()) * Constants.MinimumDistanceHeld;
        }

        /// <summary>
        /// Returns random registration number
        /// </summary>
        public static string RegistrationNumber()
        {
            int voivodeship = rnd.Next(1, 17);
            string registration = string.Empty;
            switch (voivodeship)
            {
                case 1:
                    registration += 'G';
                    break;
                case 2:
                    registration += 'Z';
                    break;
                case 3:
                    registration += 'N';
                    break;
                case 4:
                    registration += 'B';
                    break;
                case 5:
                    registration += 'C';
                    break;
                case 6:
                    registration += 'P';
                    break;
                case 7:
                    registration += 'F';
                    break;
                case 8:
                    registration += 'D';
                    break;
                case 9:
                    registration += 'O';
                    break;
                case 10:
                    registration += 'S';
                    break;
                case 11:
                    registration += 'E';
                    break;
                case 12:
                    registration += 'T';
                    break;
                case 13:
                    registration += 'K';
                    break;
                case 14:
                    registration += 'R';
                    break;
                case 15:
                    registration += 'L';
                    break;
                case 16:
                    registration += 'W';
                    break;
                default:
                    return "Error";
            }
            registration += (char)('A' + rnd.Next(0, 26));
            registration += (char)('A' + rnd.Next(0, 26));
            registration += ' ';
            for (int i = 0; i < 3; i++)
            {
                if (rnd.Next(10) % 2 == 1)
                    registration += (char)(48 + rnd.Next(0, 10));
                else
                    registration += (char)('A' + rnd.Next(0, 26));
            }
            return registration;
        }
    }
}
