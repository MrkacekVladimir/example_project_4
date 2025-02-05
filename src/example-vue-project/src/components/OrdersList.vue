<template>
  <div>
    <h1>Orders List</h1>
    <table v-if="orders.length > 0">
      <thead>
        <tr>
          <th>Id</th>
          <th>Status</th>
          <th>Created On</th>
          <th>Paid On</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="order in orders" :key="order.Id">
          <td>{{ order.id }}</td>
          <td>{{ order.status }}</td>
          <td>{{ formatDate(order.createdOn) }}</td>
          <td>{{ order.paidOn ? formatDate(order.paidOn) : 'N/A' }}</td>
        </tr>
      </tbody>
    </table>
    <p v-else>No orders available</p>
  </div>
</template>

<script setup lang="tsx">
import { ref, onMounted } from "vue";

const orders = ref([] as any[]);

// Function to fetch orders
const fetchOrders = async () => {
  try {
    const response = await fetch('https://localhost:7271/api/orders');
    if (!response.ok) {
      throw new Error('Failed to fetch orders');
    }
    const data = await response.json();
    orders.value = data;
  } catch (error) {
    console.error('Error fetching orders:', error);
  }
};

// Function to format dates
const formatDate = (dateString: string) => {
  const date = new Date(dateString);
  return date.toLocaleString();
};

// Fetch orders on component mount
onMounted(() => {
  fetchOrders();
});

</script>

<style scoped>
table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  border: 1px solid #ddd;
  padding: 8px;
  text-align: left;
}

th {
  background-color: #f4f4f4;
}

td {
  background-color: #fafafa;
}
</style>
