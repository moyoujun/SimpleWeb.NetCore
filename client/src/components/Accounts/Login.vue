<template>
  <section class="absolute w-full h-full top-0">
    <div
      class="absolute top-0 w-full h-full bg-gray-900"
      style="background-size: 100%; background-repeat: no-repeat"
    ></div>
    <div class="container mx-auto px-4 h-full">
      <div class="flex content-center items-center justify-center h-full">
        <div class="w-full lg:w-4/12 px-4 pt-32">
          <div
            class="relative flex flex-col min-w-0 break-words w-full mb-6 shadow-lg rounded-lg bg-gray-300 border-0"
          >
            <div class="flex-auto px-4 lg:px-10 py-10 pt-0">
              <div class="text-gray-500 text-center mb-3 font-bold">
                <small>sign in with credentials</small>
              </div>
              <form action="">
                <div class="relative w-full mb-3">
                  <label
                    class="block uppercase text-gray-700 text-xs font-bold mb-2"
                    for="grid-password"
                    >Email</label
                  >
                  <input
                    type="email"
                    class="px-3 py-3 placeholder-gray-400 text-gray-700 bg-white rounded text-sm shadow focus:outline-none focus:shadow-outline w-full"
                    placeholder="Email"
                    v-model="email"
                    style="transition: all 0.15s ease 0s"
                  />
                </div>
                <div class="relative w-full mb-3">
                  <label
                    class="block uppercase text-gray-700 text-xs font-bold mb-2"
                    for="grid-password"
                    >Password</label
                  >
                  <input
                    type="password"
                    class="px-3 py-3 placeholder-gray-400 text-gray-700 bg-white rounded text-sm shadow focus:outline-none focus:shadow-outline w-full"
                    placeholder="Password"
                    v-model="password"
                    style="transition: all 0.15s ease 0s"
                  />
                </div>
                <div>
                  <label class="inline-flex items-center cursor-pointer">
                    <input
                      id="customCheckLogin"
                      type="checkbox"
                      class="form-checkbox text-gray-800 ml-1 w-5 h-5"
                      style="transition: all 0.15s ease 0s"
                    /><span class="ml-2 text-sm font-semibold text-gray-700"
                      >Remember me</span
                    ></label
                  >
                </div>
                <div class="text-center mt-6">
                  <button
                    class="bg-gray-900 text-white active:bg-gray-700 text-sm font-bold uppercase px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 w-full"
                    @click.prevent="onLoginFomSubmit"
                    style="transition: all 0.15s ease 0s"
                  >
                    Sign In
                  </button>
                </div>
              </form>
            </div>
          </div>
          <div class="flex flex-wrap mt-6">
            <div class="w-1/2">
              <a href="#pablo" class="text-gray-300"
                ><small>Forgot password?</small></a
              >
            </div>
            <div class="w-1/2 text-right">
              <a href="#pablo" class="text-gray-300"
                ><small>Create new account</small></a
              >
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import axios from "axios";

export default defineComponent({
  setup() {
    const email = ref<string>();
    const password = ref<string>();

    const onLoginFomSubmit = async () => {
      const response = await axios.post("/api/Account/Login", {
        email: email.value,
        password: password.value,
      });
      alert("Hello!" + response);
    };

    return {
      email,
      password,
      onLoginFomSubmit,
    };
  },
});
</script>


<style scoped lang="postcss">
</style>