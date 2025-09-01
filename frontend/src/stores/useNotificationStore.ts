import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useNotificationStore = defineStore('notification', () => {
  const show = ref(false)
  const message = ref('')
  const color = ref('success')

  function showSuccess(msg: string) {
    message.value = msg
    color.value = 'success'
    show.value = true
  }

  function showError(msg: string) {
    message.value = msg
    color.value = 'error'
    show.value = true
  }

  return {
    show,
    message,
    color,
    showSuccess,
    showError
  }
})
