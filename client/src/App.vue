<template>
  <div>
    <router-view />
  </div>
</template>

<style lang="postcss">
@tailwind base;
@tailwind components;
@tailwind utilities;
@tailwind screens;

.btn-primary {
  @apply bg-blue-500 text-gray-200 text-sm font-bold  px-6 py-3 rounded shadow outline-none;
}

.btn-primary:hover {
  @apply bg-blue-200;
}

.btn-primary:focus {
  @apply outline-none;
}
</style>


