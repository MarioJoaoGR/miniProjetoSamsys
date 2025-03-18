import axios from "axios";

const getAuthors = "http://localhost:5000/api/Author/GetAll";



export const getAllAuthors = async () => {
  const response = await axios.get(`${getAuthors}`);
  return response.data;
};





