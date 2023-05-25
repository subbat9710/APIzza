<template>
     <div class="weather-data" :style="{ backgroundImage: weatherBackgroundImage }">
      <div class="weather-content">
        <p> {{ weatherData.locationName }}, {{ weatherData.region }}</p>
        <p> {{ weatherData.text }} & {{ weatherData.temp_f }} °F</p>
        <p>{{ temperatureMessage }}</p>
        <img :src="weatherData.icon">
      </div>
    </div>
</template>

<script>
import axios from "axios";

export default {
  name: "WeatherData",
  props: ["isDelivery"],
  data() {
    return {
      weatherData: {},
      weatherHeight: 0,
    };
  },
  computed: {
    temperatureMessage() {
      const temp = this.weatherData.temp_f;
      if (temp < 32) {
        return "It's the perfect day to stay in and order a hot pizza!";
      } else if (temp >= 32 && temp < 60) {
        return "It's chilly outside. Don't forget to add some toasty bread bits!";
      } else if (temp >= 60 && temp < 80) {
        return "The weather's perfect. Enjoy an ice cold beverage with your pizza!";
      } else {
        return "Just order a pizza, you know you want to";
      }
    },
    weatherBackgroundImage() {
      if (!this.weatherData.text) {
        return "";
      }

      const weatherCondition = this.weatherData.text.toLowerCase();
      let backgroundImage = "";

      switch (weatherCondition) {
        case "sunny":
          backgroundImage =
            "url('https://64.media.tumblr.com/c4205bb1f4230f38c8e79b49055a9c67/b4d4f2e5ea6f4d8e-78/s640x960/a14a1b8f48c5d728c499c6ca9f2922003866bb63.gif')";
          break;
        case "raining":
          backgroundImage =
            "url('https://gifdb.com/images/high/aesthetic-anime-heavy-rain-splash-jeajbmb2ohg1x7z3.gif')";
          break;
        case "partly cloudy":
          backgroundImage =
            "url('https://thumbs.gfycat.com/DeadlyEmotionalArcherfish-size_restricted.gif')";
          break;
        case "light rain":
          backgroundImage =
            "url('https://www.icegif.com/wp-content/uploads/rain-icegif-1.gif')";
          break;
        case "thunder storm":
          backgroundImage =
            "url('https://media3.giphy.com/media/FZzbTJyRTwPuw/giphy.gif')";
          break;
        case "overcast":
          backgroundImage =
            "url('https://www.adventurebikerider.com/wp-content/uploads/2017/10/tumblr_o27c7fByaO1tchrkco1_500.gif')";
          break;
        case "clear":
          backgroundImage =
            "url('https://thumbs.gfycat.com/ForcefulSpiffyAppaloosa-max-1mb.gif')";
          break;
        default:
          backgroundImage = "url('https://example.com/default.gif')";
          break;
      }

      return backgroundImage;
    },
  },
  mounted() {
this.fetchWeather();
},
methods: {
fetchWeather() {
  this.weatherData = {};
axios
.get("https://localhost:44315/api/weather/Pittsburgh")
.then((response) => {
console.log("Weather data:", response.data);
this.weatherData = response.data;
})
.catch((error) => {
console.error("Error fetching weather data:", error);
});
},
},
};
</script>

<style scoped>
.container {
  text-align: center;
}

.weather-data {
  background-repeat: no-repeat;
  background-size:  auto;
  background-position: center;
  position: fixed;
  top: 20px;
  right: 20px;
  width: 280px;
  height: 230px;
  border-radius: 10px;
  box-shadow: 0 8px 15px rgba(0, 0, 0, 0.1);
  z-index: 1000;
  
}

.weather-content {
  position: absolute;
  z-index: 1;
  border-radius: 10px;
  text-align: center;
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  font-size: 0.3cm;
  word-wrap: break-word;
  
}

.header {
  margin-bottom: 2rem;
}

.subtitle {
  font-size: 1.25rem;
  font-weight: normal;
  margin-top: -0.5rem;
}

</style>
