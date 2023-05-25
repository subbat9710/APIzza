using System.ComponentModel.DataAnnotations;

namespace APIzza.Models.Account
{
    public class RegisterUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        // a little data validation
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }

        public List<string> ValidatePassword()
        {
            List<string> failedRules = new List<string>();

            if (Password.Length < 10)
            {
                failedRules.Add("Password must be greater then 10 characters");
            }

            if (Password.ToLower().Contains("password"))
            {
                failedRules.Add("Shouldn't include word 'Password'");
            }
            if (Password.ToLower().Contains(Username))
            {
                failedRules.Add("Password should not include username");
            }
            Dictionary<char, int> charCounts = new Dictionary<char, int>();
            foreach (char c in Password)
            {
                if (charCounts.ContainsKey(c))
                {
                    charCounts[c]++;
                }
                else
                {
                    charCounts[c] = 1;
                }

                if (charCounts[c] > 4)
                {
                    failedRules.Add("Shouldn't have same characters repeated for 4 times");
                    break;
                }
            }

            if (!Password.Any(char.IsUpper) || !Password.Any(char.IsLower))
            {
                failedRules.Add("Password should include atleast one uppercase or lowercase");
            }

            if (!Password.Contains("*") && !Password.Contains("#") && !Password.Contains("@") && !Password.Contains("$") && !Password.Contains("%"))
            {
                failedRules.Add("Password should include special characters *,@,#,$,%");
            }

            if (!Password.Any(char.IsDigit))
            {
                failedRules.Add("Password should include at least one number (0-9)");
            }
            return failedRules;
        }
    }
}
