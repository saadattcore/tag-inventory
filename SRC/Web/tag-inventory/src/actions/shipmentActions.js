import {
  GET_SHIPMENT,
  GET_SHIPMENTS,
  RESET_SHIPMENTS,
  ADD_UPDATE_SHIPMENT
} from "./actionTypes";
import http from "../services/HttpModule";

export const getShipments = url => async dispatch => {
  try {
    const response = await http.get(url);

    console.log(response);

    const { data, totalCount, searchCount } = response.data;

    console.log(data);

    dispatch({
      type: GET_SHIPMENTS,
      payload: { shipments: data, totalCount, searchCount }
    });
  } catch (ex) {
    console.log(ex);

    alert("error");
    dispatch({
      type: RESET_SHIPMENTS
    });
  }
};

export const getShipment = url => async dispatch => {
  return new Promise(async (resolve, reject) => {
    const { data } = await http.get(url);

    dispatch({
      type: GET_SHIPMENT,
      payload: { shipment: data }
    });

    resolve(data);
  });
};

export const addUpdateShipment = (url, shipment) => async disptach => {
  return new Promise(async (resolved, reject) => {
    try {
      const response =
        shipment.shipmentID === -1
          ? await http.post(url, shipment)
          : await http.put(url, shipment);
      disptach({ type: ADD_UPDATE_SHIPMENT, payload: { shipment } });
      resolved(response);
    } catch (ex) {
      console.log(ex);
      reject(ex.response);
    }
  });
};

export const downloadExportPackage = (url, shipmentID) => async dispatch => {
  return new Promise(async (resolve, reject) => {
    try {
      const response = await http.get(url, { responseType: "blob" });

      let downloadLink = document.createElement("a");
      downloadLink.href = window.URL.createObjectURL(response.data);
      downloadLink.setAttribute(
        "download",
        "exportpackage_" + shipmentID + ".zip"
      );
      document.body.appendChild(downloadLink);
      downloadLink.click();
      downloadLink.remove();

      /*  const n_url = new Blob([response.data], { type: "application/zip" });

      const link = document.createElement("a");

      link.href = n_url;
      link.setAttribute("download", "shipment_zip.zip");

      document.body.appendChild(link);
      link.click(); // this will download file.zip */

      /* const byteCharacters = atob(response.data);
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

      const blob = new Blob(byteArrays, { type: "application/zip" });

      var fileURL = window.URL.createObjectURL(blob);
      window.open(fileURL);
 */
      resolve(response);
    } catch (error) {
      /*  console.log(error.response.data);
      console.log(error.response.status);
      console.log(error.response.headers);
      console.log(error.request);
      alert(error.response.body);
      console.log("Error", error.message);
 */
      reject(error);
    }
  });
};
