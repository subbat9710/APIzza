<template>
  <div class="check-out">
    <h1>CHECK OUT</h1>
    <final-check-out></final-check-out>
  </div>
</template>

<script>
import FinalCheckOut from "@/components/FinalCheckOut.vue";

export default {
  name: "CheckOut",
  components: {
    FinalCheckOut,
  },
};
</script>

<style scoped>
.check-out {
  width: 100%;
  padding: 20px;
  box-sizing: border-box;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
}

h1 {
  text-align: center;
  margin-bottom: 20px;
  width: 100%;
}

.final-check-out {
  width: 100%;
  max-width: 800px;
}
</style>
