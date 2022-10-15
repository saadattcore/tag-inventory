import { display } from "@material-ui/system";
import { reject } from "lodash";
import {
  GET_TAGS,
  RESET_TAGS,
  GET_TAG,
  GET_TAG_HISTORY,
  UPDATE_TAG,
} from "../actions/actionTypes";
import http from "../services/HttpModule";

export const getTags = (url) => async (dispatch) => {
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.get(url);
      const { data, totalCount, searchCount } = response.data;
      const tranformedTags = data.map((tag) => {
        const tagClone = { ...tag };
        tagClone["isImported"] = tag["isImported"] === true ? "Yes" : "No";
        return tagClone;
      });
      dispatch({
        type: GET_TAGS,
        payload: { tags: tranformedTags, totalCount, searchCount },
      });

      resolve(tranformedTags);
    } catch (ex) {
      console.log(ex.response);
      dispatch({ type: RESET_TAGS });
      reject(ex);
    }
  });
};

export const getTag = (url) => async (dispatch) => {
  return new Promise(async (resolve, reject) => {
    http
      .get(url)
      .then((response) => {
        const { data } = response.data;
        const tagClone = {
          ...response.data,
          receivedBox: {
            ...response.data.receivedBox,
            shipment: { ...response.data.receivedBox.shipment },
          },
        };

        dispatch({
          type: GET_TAG,
          payload: { tag: tagClone },
        });

        resolve(tagClone);
      })
      .catch((error) => {
        console.log(error.response);
        reject(error);
      });
  });
};

export const getTagHistory = (url) => async (dispatch) => {
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.get(url);
      console.log(response.data);
      dispatch({ type: GET_TAG_HISTORY, payload: response.data });
      resolve(response);
    } catch (ex) {}
  });
};

export const UpdateTag = (url, tag) => async (dispatch) => {
  console.log(tag);
  return new Promise(async (resolve, reject) => {
    try {
      await http.put(url, tag);
      dispatch({ type: UPDATE_TAG, payload: { tag } });
      resolve(tag);
    } catch (ex) {
      reject(ex);
    }
  });
};
