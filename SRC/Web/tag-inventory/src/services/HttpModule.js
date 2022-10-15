import axios from "axios";
import data from "./fake/repository.json";
import paginate from "../utils/paging";

export class HttpClient {
  getData(url, pageConfig) {
    const { startIndex, endIndex } = paginate(pageConfig);
    return {
      data: data.slice(startIndex, endIndex + 1),
      itemsCount: data.length
    };
  }

  getFilteredData(path, value) {
    const objSchema = data[0];
    const propValue = objSchema[path];

    console.log(propValue);

    return isNaN(propValue)
      ? data.filter(item =>
          item[path].toLowerCase().includes(value.toLowerCase())
        )
      : data.filter(item => item[path] === parseInt(value));
  }
  /*
    console.log(url);
    axios
      .get(url)
      .then(response => {
        console.log(response.data);
      })
      .catch(error => {
        console.log(error);
      });

      */
}

export default {
  get: axios.get,
  post: axios.post,
  put: axios.put,
  patch: axios.patch,
  delete: axios.delete
};
