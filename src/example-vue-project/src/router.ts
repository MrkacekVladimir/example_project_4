import { createRouter, createWebHistory } from "vue-router"
import OrdersListPage from './pages/orders/OrdersListPage.vue'
import LoginPage from './pages/auth/LoginPage.vue'

const routes = [
  { path: '/orders', component: OrdersListPage },
  { path: '/login', component: LoginPage },
]

export const router = createRouter({
  history: createWebHistory(),
  routes,
})

//TODO: Add auth guard
router.beforeEach((to, from) => {
  console.log(to, from);
})