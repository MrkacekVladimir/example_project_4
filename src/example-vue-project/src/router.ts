import { createRouter, createWebHistory } from "vue-router"
import OrdersList from './components/OrdersList.vue'
import LoginPage from './pages/auth/LoginPage.vue'

const routes = [
  { path: '/orders', component: OrdersList },
  { path: '/login', component: LoginPage },
]

export const router = createRouter({
  history: createWebHistory(),
  routes,
})