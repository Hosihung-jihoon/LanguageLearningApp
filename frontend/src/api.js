import axios from 'axios';

const api = axios.create({
    baseURL: "http://localhost:5194/api",
});

export default api;