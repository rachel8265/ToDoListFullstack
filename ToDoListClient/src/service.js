import axios from 'axios';

axios.defaults.baseURL = process.env.REACT_APP_API_URL

axios.interceptors.response.use(
  response => response, 
  error => {
    console.error('API Error:', {
      message: error.message,
      status: error.response?.status,
      data: error.response?.data
    });
    return Promise.reject(error); 
  }
);


export default {
  getTasks: async () => {
    const result = await axios.get(`/items`)    
    if (Array.isArray(result.data))
      return result.data
      else {
        return [];
      }
  },

  addTask: async(name)=>{
    console.log('addTask', name)
   
    const result = await axios.post(`/items`,{name:name,isComplete:false})    
    return result;
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    const result = await axios.put(`/items/${id}`,{isComplete:isComplete})    

    return {};
  },

  deleteTask:async(id)=>{
    console.log('deleteTask')
    const result = await axios.delete(`/items/${id}`)    

  }
  
};
 