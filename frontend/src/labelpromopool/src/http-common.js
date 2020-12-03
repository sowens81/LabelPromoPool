import axios from "axios";

export default axios.create({
    baseURL: "http://localhost:63461/api/v1.0",
    headers: {
      "Content-type": "application/json"
    }
  });