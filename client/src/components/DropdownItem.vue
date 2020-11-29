<template>
  <div class="dropdown-item">
    <slot></slot>
  </div>
</template>

<style scoped lang="postcss">
.dropdown-item {
  @apply px-4 py-2 text-sm leading-5 text-gray-700 block;
}

.dropdown-item:hover {
  @apply bg-gray-100 text-gray-900 cursor-pointer;
}

.dropdown-item:focus {
  @apply outline-none bg-gray-100 text-gray-900;
}
</style>