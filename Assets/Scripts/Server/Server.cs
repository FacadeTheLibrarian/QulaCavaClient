using UnityEngine;
internal static class Server {
    public const int DEFAULT_TIMEOUT = 5;

    public const int OKAY = 200;
    public const int NOT_FOUND = 404;

    public const string HOME_DIRECTORY = "http://127.0.0.1/qula-cava/";
    public const string LOG_IN = "login.php";

    public const string CONTENT_TYPE = "Content-Type";
    public const string CONTENT_TYPE_JSON = "application/json";
    public const string CONTENT_TYPE_X_WWW_FORM = "application/x-www-form-urlencoded";

    public const string USERNAME = "username";
    public const string PASSWORD = "password";
}