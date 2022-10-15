import {
  GET_RECEIVED_BOXES,
  GET_RECEIVED_BOX,
  RESET_RECEIVED_BOXES_STATE,
  UPDATE_RECEIVED_BOX,
  UPDATE_SCAN_TAGS,
  ISSUED_BOX_SCAN_TAG,
} from "./actionTypes";
import http from "../services/HttpModule";

export const getReceivedBoxes = (url) => async (dispatch) => {
  try {
    const response = await http.get(url);
    const { data, totalCount, searchCount } = response.data;
    dispatch({
      type: GET_RECEIVED_BOXES,
      payload: {
        receivedBoxes: data,
        totalCount: totalCount,
        searchCount: searchCount,
      },
    });
  } catch (ex) {
    dispatch({
      type: RESET_RECEIVED_BOXES_STATE,
    });
  }
};

/* export const getReceivedBox = url => async dispatch => {
  const response = await http.get(url);
  const receivedBox = response.data;

  dispatch({
    type: GET_RECEIVED_BOX,
    payload: {
      receivedBox: receivedBox
    }
  });
}; */

export const getReceivedBox = (url, receivedBox) => async (dispatch) => {
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.get(url);
      dispatch({
        type: GET_RECEIVED_BOX,
        payload: { receivedBox: response.data },
      });
      resolve(response.data);
    } catch (ex) {
      reject(ex.response);
    }
  });
};

export const updateReceivedBox = (url, receivedBox) => async (dispatch) => {
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.put(url, receivedBox);
      dispatch({ type: UPDATE_RECEIVED_BOX, payload: { receivedBox } });
      resolve(response);
    } catch (ex) {
      reject(ex.response);
    }
  });
};

export const updateScanBoxTags = (url, receivedBox) => async (dispatch) => {
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.put(url, receivedBox);
      dispatch({ type: UPDATE_SCAN_TAGS, payload: { response } });
      resolve(response);
    } catch (ex) {
      reject(ex.response);
    }
  });
};

export const addIssuedBoxScanTag = (scanTag) => async (disptach) => {
  return new Promise((resolve, reject) => {
    disptach({ type: ISSUED_BOX_SCAN_TAG, payload: { scanTag: scanTag } });
    resolve(scanTag);
  });
};
