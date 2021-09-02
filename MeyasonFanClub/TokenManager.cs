namespace MeyasonFanClub {

    public class TokenManager {

        public TokenManager(){
            DiscordToken = "";
            R6SToken = "";
        }

        public string DiscordToken { get; private set; }

        public string R6SToken { get; private set; }

    }
}