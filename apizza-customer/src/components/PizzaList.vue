<template>
  <div class="container">
    <div class="below-corner-image"></div>
    <div class="header-container">
      <div class="corner-image"></div> 
      <div class="logo" ref='threeContainer'>
      </div>
    </div>
  </div>
</template>

<script>
import * as THREE from 'three' ;

export default {
  name: "PizzaList",
  data() {
    return {
      logoUrl: "",
      scene: null,
      camera: null,
      renderer: null,
      animationFrameID: null,
      cube: null,
      geometry: null,
    };
  },
  created(){
    this.fetchLogo();
  },
  mounted() {
    //this.fetchLogo();
    this.initThreeScene();
  },
  beforeUnmount() {
    // Clean up resources
    cancelAnimationFrame(this.animationFrameID);
    this.renderer.dispose();
  },

  methods: {
    fetchLogo() {
      fetch("https://i.imgur.com/Cqe53u6.png")
        .then((response) => {
          if (response.ok) {
            this.logoUrl = response.url;
          } else {
            throw new Error("Failed to fetch the logo.");
          }
        })
        .catch((error) => {
          console.error("Error fetching the logo:", error);
        });
    },
    initThreeScene() {
      // Set up the scene, camera, and renderer
      this.scene = new THREE.Scene();
      this.camera = new THREE.PerspectiveCamera(
        75,
        this.$refs.threeContainer.clientWidth /
          this.$refs.threeContainer.clientHeight,
        0.1,
        1000
      );
      this.renderer = new THREE.WebGLRenderer({alpha:true});
      this.renderer.setClearColor(0x000000,0);
      this.renderer.setSize(
        this.$refs.threeContainer.clientWidth,
        this.$refs.threeContainer.clientHeight
      );
      this.$refs.threeContainer.appendChild(this.renderer.domElement);

      // Add a cube to the scene
      const textureLoader = new THREE.TextureLoader();
      
      textureLoader.load("https://i.imgur.com/Cqe53u6.png", (texture) => {
        // Create a cube with the texture
        this.geometry = new THREE.PlaneGeometry(6,6);

        const material = new THREE.MeshBasicMaterial({ map: texture, transparent: true });
        this.cube = new THREE.Mesh(this.geometry, material);
        this.cube.material.side = THREE.DoubleSide;
        this.scene.add(this.cube);
        console.log('i have the texture');
      })

      this.camera.position.z = 5;

      // Start the animation loop
      this.animate();
    },
    animate() {
      this.animationFrameID = requestAnimationFrame(this.animate);
      if(this.cube!=null){
        this.cube.rotation.y += 0.0075; // Change speed of rotation on logo
      }

      // Update the scene and render
      this.renderer.render(this.scene, this.camera);
    },
  },
};
</script>

<style scoped>
.container {
  position: relative;
}

.header-container {
  position: relative;
  top: 0;
  left: 24.5%;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  z-index: 2;
}

canvas{
  position: absolute;
  top:0;
}

img{
  z-index: -1;
}
.logo {
  width: 40%;
  height: 300px;
  position: relative;
  overflow: hidden;
  background-image: '../assets/Banner_Pixel.png';
  background-size: contain;
}

.logo img {
  width: 100%;
}

@media only screen and (max-width: 400px) {
  .logo {
    width: 300px;
  }
}

.corner-image {
  position: fixed;
  top: 8px;
  left: 8px;
  width: 200px; /* Adjust the width as needed */
  height: 200px; /* Adjust the height as needed */
  background-image: url("https://i.imgur.com/oJS0XUC.png");
  background-size: 100% 100%;
  background-repeat: no-repeat;
  z-index: 1;
  border: 6px solid rgb(0, 0, 0);
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}


.below-corner-image {
  position: absolute;
  top: 210px; /* Adjust this value to position the image directly below the corner image */
  left: 0;
  width: 250px; /* Adjust the width as needed */
  height: 250px; /* Adjust the height as needed */
  background-image: url("https://i.imgur.com/SAxaw8C.png");
  background-size: 100% 100%;
  background-repeat: no-repeat;
  z-index: 0; /* Set a lower z-index to place the image behind all other elements */
}
</style>
