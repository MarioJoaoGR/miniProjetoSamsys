import axios from "axios";

const getBooks = "http://localhost:5000/api/Book/GetAllActive";
const url = "http://localhost:5000/api/Book";
const filer = "http://localhost:5000/api/Book/search";


export const getAllBooks = async () => {
  const response = await axios.get(`${getBooks}`);
  return response.data;
};

export const getBook = async (id: string) => {
  const response = await axios.get(`${url}/${id}`);
  return response.data;
};

export const createBook = async (isbn: string, title: string, authorNif: string, value: string) => {
    const response = await axios.post(url, {
      isbn,
      title,
      authorNif,
      value
    });
    return response.data;
  };

  export const editBook = async (id: string, isbn?: string, title?: string, authorNif?: string, value?: string) => {
    const body: any = {};
    body.id = id;
    if (isbn) body.isbn = isbn;
    if (title) body.title = title;
    if (authorNif) body.authorNif = authorNif;
    if (value) body.value = value;
  
    const response = await axios.put(`${url}/${id}`, body);
    return response.data;
  };



export const deleteBook = async (id: string) => {
  await axios.delete(`${url}/${id}`);
};


export const filterBooks = async (isbn?: string, title?: string, authorName?: string, valueOrder?: string) => {
    let params = new URLSearchParams();
    if (isbn) {
      params.append('isbn', isbn);
    }
    if (title) {
      params.append('title', title);
    }
    if (authorName) {
      params.append('authorName', authorName);
    }
    if (valueOrder) {
      params.append('valueOrder', valueOrder);
    }
  
    const response = await axios.get(`${filer}`, { params });
    return response.data;
  };


