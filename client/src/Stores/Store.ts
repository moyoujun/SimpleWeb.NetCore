import { readonly } from 'vue'
import { createAction } from './Actions'
import { createState } from './State'

const state = createState()
const action = createAction(state)

export const useStore = () => {
  const store = {
    state: readonly(state),
    action: readonly(action)
  }

  return store
}
