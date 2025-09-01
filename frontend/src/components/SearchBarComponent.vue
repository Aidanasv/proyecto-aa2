<script setup lang="ts">
import { defineEmits, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const emit = defineEmits(['update:query'])

const query = ref('')

watch(query, (val) => {
  emit('update:query', val.trim())
})
</script>

<template>
  <v-text-field
    v-model="query"
    :label="t('search')"
    prepend-inner-icon="mdi-magnify"
    variant="solo-filled"
    density="compact"
    hide-details
    flat
    class="search-bar"
  />
</template>

<style scoped>
.search-bar {
  min-width: 200px;
  max-width: 400px;
}
</style>
