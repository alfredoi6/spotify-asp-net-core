# Spotify API Integration Demo

This project demonstrates how to integrate the **Spotify API** into a web application to explore and leverage user musical preferences. By exercising multiple endpoints provided by Spotify's API, this application showcases how developers can fetch and analyze user data, opening up a wide range of possibilities for personalized applications.

---

## ✨ Features

- **User Authorization**  
  Authenticate users directly with Spotify via OAuth 2.0, gaining secure access to their music profile.

- **User's Top Tracks and Artists**  
  Fetch and display the user's favorite tracks and artists to better understand their listening habits.

- **Genre Analysis**  
  Analyze music genres based on user preferences, helping developers deliver targeted recommendations or insights.

- **Recently Played Tracks**  
  Retrieve the user's playback history and discover their latest listening trends.

- **Audio Features & Analysis**  
  Fetch enriched audio details (e.g., danceability, energy, valence) for tracks, enabling deeper insights into their preferences.

---

## 🚀 Use Cases

This integration aims to inspire developers by illustrating how Spotify's APIs can be utilized effectively. Some practical real-world applications include:

- **Personalized Playlists**  
  Generate custom playlists for users based on their unique music taste.

- **Music Discovery Platforms**  
  Suggest new music that aligns with the user's listening habits and preferences.

- **Event & Venue Recommendations**  
  Recommend concerts, music festivals, or clubs catering to the user's favorite genres and artists.

- **Mood-Based Applications**  
  Use audio features like tempo, energy, and valence to suggest music to match or enhance a user's mood.

---

## 🛠️ Technologies Used

- **ASP.NET Core MVC**  
  A lightweight and dynamic framework for building model-view-controller web applications.

- **C# 13.0**  
  Utilizing the latest features of C# for clean and maintainable code.

- **Spotify Web API**  
  The heart of this project, connecting the app to Spotify's vast library of music and user data.

---

## 🔧 Integration Steps

To replicate or extend this project using the Spotify API, follow these steps:

### 1. Create a Spotify Developer Application

1. Go to the [Spotify Developer Dashboard](https://developer.spotify.com/dashboard/).
2. Log in with your Spotify account.
3. Click on **Create an App** and configure it:
   - App Name: _Your App's Name_
   - Description: _Your App’s Description_
4. Save your Client ID and Client Secret, which will be used for authentication.

---

### 2. Configure App Settings  

Update the `appsettings.json` file or environment variables in your application with the following:

```json
{
  "Spotify": {
    "ClientId": "your-spotify-client-id",
    "ClientSecret": "your-spotify-client-secret",
    "RedirectUri": "your-app-redirect-url"
  }
}
```

Ensure your redirect URL matches what you set in your Spotify Developer app.

---

### 3. Authenticate Users  

Implement the OAuth 2.0 flow using Spotify's authorization endpoint. Users will log in and grant your application access to their data.

Spotify provides the following scopes for various permissions, such as:

- `user-top-read` for top artists and tracks.
- `user-read-recently-played` for listening history.
- `playlist-modify-public/private` for playlist modifications.

Pass the necessary scopes during authorization based on your app's requirements.

---

### 4. Query the API  
Using the Access Token obtained after authentication, query Spotify's APIs for the desired user data. Example endpoints include:

- **Get User's Top Tracks/Artists**  
  ```
  GET https://api.spotify.com/v1/me/top/{type}
  ```
- **Get Recently Played Tracks**  
  ```
  GET https://api.spotify.com/v1/me/player/recently-played
  ```
- **Get Audio Features**  
  ```
  GET https://api.spotify.com/v1/audio-features/{id}
  ```

You can extend this by combining responses from multiple endpoints to gain deeper insights into a user's preferences.

---

## 📈 Example Output

Here are some examples of the data this application can pull and insights it can generate:

### User's Favorite Artists:
```json
[
  { "name": "Taylor Swift", "genre": "Pop" },
  { "name": "Drake", "genre": "Hip-Hop" },
  { "name": "Billie Eilish", "genre": "Indie Pop" }
]
```

### Track-Level Audio Features:
```json
{
  "Danceability": 0.85,
  "Energy": 0.78,
  "Valence": 0.67,
  "Tempo": 120
}
```

These insights help developers understand user behavior and preferences, enabling personalized experiences.

---

## 🧑‍💻 Development & Running the Application

### Prerequisites:
- .NET 9.0 SDK
- A registered Spotify Developer App

### Running Locally:
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/spotify-api-integration-demo.git
   cd spotify-api-integration-demo
   ```
2. Install dependencies and build the project:
   ```bash
   dotnet build
   ```
3. Run the application:
   ```bash
   dotnet run
   ```

---

## 🌟 Contribution

Contributions to this project are welcome! Here’s how you can get started:

1. Fork the repository and clone it locally.
2. Create a new branch for your feature: `git checkout -b feature-name`.
3. Commit your changes: `git commit -m "Add your message here"`.
4. Push to your fork and submit a Pull Request.

---

## 📜 License

This project is licensed under the [MIT License](LICENSE).

---

## 📚 Resources

- [Spotify Developer Documentation](https://developer.spotify.com/documentation/)
- [ASP.NET Core MVC Documentation](https://learn.microsoft.com/en-us/aspnet/core/mvc/?view=aspnetcore-9.0)

---

## ❤️ Support

If you find this project helpful, feel free to give it a ⭐️ or share it with others!