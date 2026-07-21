import axios from 'axios';

// Vercel / Production: Sử dụng biến môi trường VITE_API_BASE_URL cấu hình trên Vercel Dashboard.
// Nếu không cấu hình, sẽ fallback về link Render mặc định hoặc localhost ở development.
const baseURL = import.meta.env.VITE_API_BASE_URL || (import.meta.env.PROD 
  ? 'https://language-learning-api.onrender.com/api' 
  : 'http://localhost:5194/api');

const api = axios.create({
  baseURL,
});

export default api;