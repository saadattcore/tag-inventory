import React, { Component, useEffect, useState } from "react";

import {
  TextField,
  Select,
  MenuItem,
  InputLabel,
  FormControl,
  Button,
} from "@material-ui/core";
import DatePicker from "@material-ui/lab/DatePicker";
import AdapterDateFns from "@material-ui/lab/AdapterDateFns";
import LocalizationProvider from "@material-ui/lab/LocalizationProvider";

const ReceivedBoxDetail = ({
  receivedBox,
  readOnly,
  onChange,
  navigateFrom,
}) => {
  console.log(typeof typeID);

  /*   const [boxTypeID, setBoxTypeID] = useState(0);
  const [boxStatusID, setBoxStatusID] = useState(0);

  setBoxTypeID(typeID);
  setBoxStatusID(statusID); */

  return (
    <fieldset className="border p-2 area ">
      <legend class="w-auto spacing float-none">{"Received Box"}</legend>
      <div
        style={{
          paddingTop: 5,
          paddingRight: 25,
          paddingBottom: 35,
          paddingLeft: 25,
        }}
      >
        <div className="row top-buffer">
          <div className="col-md-4">
            <label>Received Box ID</label>
          </div>
          <div className="col-md-8">
            <FormControl fullWidth>
              <TextField
                variant="standard"
                value={receivedBox.receivedBoxID}
                size="small"
                name="receivedBoxID"
                disabled={readOnly}
                InputLabelProps={{
                  shrink: true,
                }}
              ></TextField>
            </FormControl>
          </div>
        </div>
        <div className="row top-buffer">
          <div className="col-md-4">
            <label>Type</label>
          </div>
          <div className="col-md-8">
            <FormControl variant="outlined" fullWidth>
              <Select
                labelId="demo-simple-select-label"
                id="boxTypeID"
                name="boxTypeID"
                value={receivedBox.boxTypeID ? receivedBox.boxTypeID : ""}
                label="Received Box Type"
                disabled={!navigateFrom}
                onChange={(e) =>
                  onChange({
                    target: { name: "boxTypeID", value: e.target.value },
                  })
                }
                variant="standard"
                style={{ padding: 0 }}
              >
                <MenuItem value={1}>New</MenuItem>
                <MenuItem value={2}>Spare</MenuItem>
              </Select>
            </FormControl>
          </div>
        </div>
        <div className="row top-buffer">
          <div className="col-md-4">
            <label>Start From</label>
          </div>
          <div className="col-md-8">
            <FormControl fullWidth>
              <TextField
                variant="standard"
                value={receivedBox.startTag}
                size="small"
                name="startTag"
                disabled={readOnly}
                InputLabelProps={{
                  shrink: true,
                }}
              ></TextField>
            </FormControl>
          </div>
        </div>
        <div className="row top-buffer">
          <div className="col-md-4">
            <label>End To</label>
          </div>
          <div className="col-md-8">
            <FormControl fullWidth>
              <TextField
                variant="standard"
                value={receivedBox.endTag}
                size="small"
                name="endTag"
                disabled={readOnly}
                InputLabelProps={{
                  shrink: true,
                }}
              ></TextField>
            </FormControl>
          </div>
        </div>
        <div className="row top-buffer">
          <div className="col-md-4">
            <label>Status</label>
          </div>
          <div className="col-md-8">
            <FormControl variant="outlined" fullWidth>
              <Select
                labelId="demo-simple-select-label"
                id="statusID"
                name="statusID"
                value={
                  receivedBox.statusID || receivedBox.statusID === 0
                    ? receivedBox.statusID
                    : " "
                }
                label="Received Box Status"
                onChange={(e) =>
                  onChange({
                    target: { name: "statusID", value: e.target.value },
                  })
                }
                disabled={!navigateFrom}
                variant="standard"
              >
                <MenuItem value={0}>Imported</MenuItem>
                <MenuItem value={1}>Delivery Verified Ok</MenuItem>
                <MenuItem value={2}>Tags Not In Delivery</MenuItem>
                <MenuItem value={3}>Additional Tags In Delivery</MenuItem>
              </Select>
            </FormControl>
          </div>
        </div>
      </div>
    </fieldset>
  );
};

export default ReceivedBoxDetail;
