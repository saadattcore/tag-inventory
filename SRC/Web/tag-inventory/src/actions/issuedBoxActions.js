import {
  GET_ISSUED_BOXES,
  RESET_ISSUED_BOXES,
  CREATE_ISSUED_BOX,
  UPDATE_ISSUED_BOX_TAGS,
  GET_ISSUED_BOX,
  DOWNLOAD_SERIAL_LIST,
  APPEND_SCANTAG_TO_ISSUEDBOX,
  APPEND_SCANTAG_TO_EXISTING_BOX,
  SELECT_ISSUED_BOX,
  GET_ISSUED_BOX_HISTORY,
  UPDATE_ISSUED_BOXES_STATUS,
  UPDATE_ISSUED_BOX
} from "./actionTypes";
import http from "../services/HttpModule";
import { get } from "http";
import { config } from "react-transition-group";

export const getIssuedBoxes = url => async dispatch => {
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.get(url);
      console.log(response);
      const { data, searchCount, totalCount } = response.data;
      console.log(data);
      dispatch({
        type: GET_ISSUED_BOXES,
        payload: {
          issuedBoxes: data,
          searchCount: searchCount,
          totalCount: totalCount
        }
      });

      resolve(data);
    } catch (ex) {
      dispatch({ type: RESET_ISSUED_BOXES });
      reject(ex);
    }
  });
};

export const createUpdateIssueBox = (url, payload) => async dispatch => {
  return new Promise(async (resolve, reject) => {
    try {
      const r = await http.post(url, payload);
      console.log(r);
      resolve("Sucess");
      payload.issuedBoxID = r.data;
      dispatch({ type: CREATE_ISSUED_BOX, payload: { issuedBox: payload } });
    } catch (ex) {
      reject(ex);
    }
  });
};

export const updateIssueBoxTags = (url, payload) => async dispatch => {
  return new Promise(async (resolve, reject) => {
    try {
      const r = await http.put(url, payload);
      resolve("Sucess");
      dispatch({
        type: UPDATE_ISSUED_BOX_TAGS,
        payload: { issuedBox: payload }
      });
    } catch (ex) {
      reject(ex);
    }
  });
};

export const getIssuedBox = url => async dispatch => {
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.get(url);
      console.log(response);
      const { data } = response;
      console.log(data);
      dispatch({
        type: GET_ISSUED_BOX,
        payload: {
          issuedBox: data
        }
      });
      resolve(data);
    } catch (error) {
      console.log(error);
      console.log(error.response);
      reject(error);
    }
  });
};

export const downloadSerialList = url => async dispatch => {
  return new Promise(async (resolve, reject) => {
    try {
      let response = await http.get(url);
      const tlink = document.createElement("a");
      tlink.href =
        "data:application/octet-stream;charset=utf-8;base64," + response.data;

      tlink.target = "_blank";
      tlink.download = `serial_list_${new Date().toUTCString()}.xlsx`;
      tlink.click();

      resolve("OK");
    } catch (ex) {
      console.log(ex);
      reject(ex);
    }
  });
};

export const appendScanTag = scanTag => dispatch => {
  return new Promise((resolve, reject) => {
    const data = {
      type: APPEND_SCANTAG_TO_ISSUEDBOX,
      payload: { scanTag: scanTag }
    };
    dispatch(data);
    resolve(scanTag);
  });
};

export const appendScanTagExisting = scanTag => dispatch => {
  return new Promise((resolve, reject) => {
    const data = {
      type: APPEND_SCANTAG_TO_EXISTING_BOX,
      payload: { scanTag: scanTag }
    };
    dispatch(data);
    resolve(scanTag);
  });
};

export const printLabel = (url, issuedBoxList) => async dispatch => {
  return new Promise(async (resolve, reject) => {
    try {
      const payload = JSON.stringify(issuedBoxList);
      console.log(payload);
      console.log(issuedBoxList);
      const response = await http.put(url, issuedBoxList);

      console.log(response.data);

      const byteCharacters = atob(response.data);
      const byteArrays = [];

      for (let offset = 0; offset < byteCharacters.length; offset += 512) {
        const slice = byteCharacters.slice(offset, offset + 512);

        const byteNumbers = new Array(slice.length);
        for (let i = 0; i < slice.length; i++) {
          byteNumbers[i] = slice.charCodeAt(i);
        }

        const byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
      }

      const blob = new Blob(byteArrays, { type: "application/pdf" });

      var fileURL = window.URL.createObjectURL(blob);
      window.open(fileURL);

      resolve(response);
    } catch (ex) {}
  });
};

export const selectIssuedBox = (box, all, checked) => async dispatch => {
  return new Promise(async (resolve, reject) => {
    console.log(box);
    dispatch({ type: SELECT_ISSUED_BOX, payload: { box, all, checked } });
    resolve("Box Selected Property Updated");
  });
};

export const updateIssuedBoxesStatus = (url, boxList) => async dispatch => {
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.put(url, boxList);
      console.log(boxList);
      dispatch({ type: UPDATE_ISSUED_BOXES_STATUS, payload: boxList });
      resolve(response);
    } catch (ex) {
      reject(ex);
    }
  });
};

export const getIssuedBoxTimeLine = url => async disptach => {
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.get(url);
      console.log(response);
      disptach({ type: GET_ISSUED_BOX_HISTORY, payload: response.data });
      resolve(response);
    } catch (ex) {
      reject(ex);
    }
  });
};

export const updateIssuedBox = (url, issuedBox) => async disptach => {
  console.log(issuedBox);
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.put(url, issuedBox);
      console.log(response);
      disptach({ type: UPDATE_ISSUED_BOX, payload: response.data });
      resolve(response);
    } catch (ex) {
      reject(ex);
    }
  });
};
