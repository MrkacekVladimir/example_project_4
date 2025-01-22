<script setup lang="ts">
import { ref } from 'vue'

defineProps<{ msg: string }>()

</script>

<template>
  
  <table>
    <thead>
        <tr>
            <th>ID</th>
            <th>Created On</th>
        </tr>
    </thead>
    <tbody>
        <tr></tr>
    </tbody>
  </table>
</template>

<style scoped>
.read-the-docs {
  color: #888;
}
</style>

