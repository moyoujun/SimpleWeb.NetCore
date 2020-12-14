<template>
  <div class="container content-center mx-auto mt-10">
    <h1 class="text-center text-xl">
      <span class="text-orange-500 font-bold text-2xl">Opps!</span>
      This is awkward... You are looking for something that doesn't actually
      exist
    </h1>
    <div class="text-center mt-10">
      <el-button class="btn-primary" type="primary" @click="btnClicked"
        >Go Back!</el-button
      >
    </div>

    <img src="../../assets/pictures/404.svg" class="mx-auto" />
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useLoading } from "vue3-loading-overlay";
import "vue3-loading-overlay/dist/vue3-loading-overlay.css";

export default defineComponent({
  setup(props, context) {
    const btnClicked = async () => {
      const loadingInstance = useLoading();
      loadingInstance.show({ loader: "dots" });
      await new Promise((resolve) => setTimeout(resolve, 2000));
      loadingInstance.hide();
    };

    return {
      btnClicked,
    };
  },
});
</script>