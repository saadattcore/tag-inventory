import { GET_DISTRIBUTORS, ADD_LOOKUP } from "./actionTypes";
import http from "../services/HttpModule";

export const getDistributors = url => async dispatch => {
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.get(url);

      dispatch({ type: GET_DISTRIBUTORS, payload: response.data });
      resolve(response);
    } catch (ex) {
      reject(ex);
    }
  });
};

export const getLookups = url => async dispatch => {
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.get(url);
      dispatch({ type: ADD_LOOKUP, payload: response.data });
      resolve(response);
    } catch (ex) {
      reject(ex);
    }
  });
};
