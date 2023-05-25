<template>
  <div id="app">
    
    <vue-particles
      style="position:fixed; width:100%; height:100%;"
      color="#FF0000"
      :particleOpacity="0.6"
      :particlesNumber="80"
      shapeType="circle"
      :particleSize="24"
      linesColor="#FFFF00"
      :linesWidth="1"
      :lineLinked="true"
      :lineOpacity="0.2"
      :linesDistance="150"
      :moveSpeed="2"
      :hoverEffect="true"
      hoverMode="grab"
      :clickEffect="true"
      clickMode="push"
    ></vue-particles>

    <section class="user-facing" style="position: relative; z-index:1;">
      <div id="nav">  
      </div>
      
      
      <router-view />
    </section>
  </div>
</template>


<script>

export default {
  name: "App",
  components: {
    
    
  },
  mounted() {
    
  }
}
</script>

<style>
* {
  cursor: url('data:image/x-icon;base64,AAACAAEAICAQAAAAAADoAgAAFgAAACgAAAAgAAAAQAAAAAEABAAAAAAAAAIAAAAAAAAAAAAAEAAAAAAAAAAAAAAANmDWAE/i8AA9oXoAUYiwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAREAAAAAAAAAAAAAAAAAAAEREAAAAAAAAAAAAAAAAAABEREAAAAAAAAAAAAAAAAAAREREAAAAAAAAAAAAAAAAAiREREAAAAAAAAAAAAAAAAIiREREAAAAAAAAAAAAAAAiIiREREAAAAAAAAAAAAAAIyIiREREAAAAAAAAAAAAAiMiMiREREAAAAAAAAAAAAIiIiIiREREAAAAAAAAAAAiIiERIiREREAAAAAAAAAAIiIhESIiREREAAAAAAAAAhESIREiIiRERAAAAAAAAAIREiIiIjIiREQAAAAAAAAiERIiMyIiIgAAAAAAAAAAIiIiIiIiIgAAAAAAAAAAARIiIiIiIgAAAAAAAAAAAAESMiESIgAAAAAAAAAAAAAREjIREQAAAAAAAAAAAAAAIiIiEQAAAAAAAAAAAAAAAiIiIgAAAAAAAAAAAAAAAAMyIgAAAAAAAAAAAAAAAAAiIgAAAAAAAAAAAAAAAAAAIgAAAAAAAAAAAAAAAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD////////////////////////////x////4P///+B////gP///4B///8AP///AB///gAP//4AB//8AAP//AAB//gAAP/4AAB/8AAAf/AAAH/gAAD/4AAP/8AAP//AAP//gAP//4AP//8AP///AP///gP///4P///8P////P////w=='), auto !important;
}

body {
  background-color: ivory;
  font-family: "Press Start 2P", cursive;
  font-weight: bold;
}

h1 {
  font-size: 2.5rem;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  margin-bottom: 1rem;
}
</style>