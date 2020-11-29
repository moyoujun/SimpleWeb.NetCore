<template>
  <div ref="dropdownMenuRef">
    <!--
  Tailwind UI components require Tailwind CSS v1.8 and the @tailwindcss/ui plugin.
  Read the documentation to get started: https://tailwindui.com/documentation
-->
    <div class="relative inline-block text-left">
      <div>
        <span class="rounded-md shadow-sm">
          <button
            type="button"
            class="inline-flex justify-center w-full rounded-md border border-gray-300 px-4 py-2 bg-white text-sm leading-5 font-medium text-gray-700 hover:text-gray-500 focus:outline-none focus:border-blue-300 focus:shadow-outline-blue active:bg-gray-50 active:text-gray-800 transition ease-in-out duration-150"
            id="options-menu"
            @click="toggleOpen"
          >
            {{ title }}
            <!-- Heroicon name: chevron-down -->
            <svg
              class="-mr-1 ml-2 h-5 w-5"
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 20 20"
              fill="currentColor"
            >
              <path
                fill-rule="evenodd"
                d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
                clip-rule="evenodd"
              />
            </svg>
          </button>
        </span>
      </div>

      <!--
    Dropdown panel, show/hide based on dropdown state.

    Entering: "transition ease-out duration-100"
      From: "transform opacity-0 scale-95"
      To: "transform opacity-100 scale-100"
    Leaving: "transition ease-in duration-75"
      From: "transform opacity-100 scale-100"
      To: "transform opacity-0 scale-95"
  -->
      <div
        class="origin-top-right absolute right-0 mt-2 rounded-md shadow-lg"
        v-if="isOpen"
      >
        <div class="rounded-md bg-white shadow-xs py-2">
          <slot></slot>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, onUnmounted, ref } from "vue";
export default defineComponent({
  props: {
    title: {
      type: String,
      required: true,
    },
  },
  setup() {
    const isOpen = ref(false);
    const dropdownMenuRef = ref<HTMLElement>();
    const toggleOpen = () => {
      isOpen.value = !isOpen.value;
    };

    const handler = (e: MouseEvent) => {
      if (!isOpen.value) {
        return;
      }

      if (dropdownMenuRef.value) {
        const val = dropdownMenuRef.value;
        const target = e.target;
        if (
          target instanceof HTMLElement &&
          !val.contains(e.target as HTMLElement)
        ) {
          isOpen.value = false;
        }
      }
    };

    onMounted(() => {
      document.addEventListener("click", handler);
    });

    onUnmounted(() => {
      document.removeEventListener("click", handler);
    });

    return {
      isOpen,
      toggleOpen,
      dropdownMenuRef,
    };
  },
});
</script>