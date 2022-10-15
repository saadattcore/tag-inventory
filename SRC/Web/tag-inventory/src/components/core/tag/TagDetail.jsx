import React, { Component, useEffect } from "react";

import {
  TextField,
  Select,
  MenuItem,
  InputLabel,
  FormControl,
  Button
} from "@material-ui/core";
import DatePicker from "@material-ui/lab/DatePicker";
import AdapterDateFns from "@material-ui/lab/AdapterDateFns";
import LocalizationProvider from "@material-ui/lab/LocalizationProvider";

const TagDetail = ({ tag, readOnly, onChange, tagStatusList }) => {
  return (
    <fieldset className="border p-2 area ">
      <legend class="w-auto spacing float-none">{"Tag"}</legend>
      <div
        style={{
          paddingTop: 5,
          paddingRight: 25,
          paddingBottom: 35,
          paddingLeft: 25
        }}
      >
        <div className="row top-buffer">
          <div className="col-md-4">
            <label>Received Box</label>
          </div>
          <div className="col-md-8">
            <FormControl fullWidth>
              <TextField
                variant="standard"
                value={tag.receivedBoxID}
                size="small"
                name="txtReceivedBox"
                InputLabelProps={{
                  shrink: true
                }}
                disabled
              ></TextField>
            </FormControl>
          </div>
        </div>
        <div className="row top-buffer">
          <div className="col-md-4">
            <label>Tag Number</label>
          </div>
          <div className="col-md-8">
            <FormControl fullWidth>
              <TextField
                variant="standard"
                value={tag.tagNumber}
                size="small"
                disabled
                name="txtTagNumber"
                InputLabelProps={{
                  shrink: true
                }}
              ></TextField>
            </FormControl>
          </div>
        </div>
        <div className="row top-buffer">
          <div className="col-md-4">
            <label>Serial Number</label>
          </div>
          <div className="col-md-8">
            <FormControl fullWidth>
              <TextField
                variant="standard"
                value={tag.serialNumber}
                size="small"
                name="txtSerialNumber"
                InputLabelProps={{
                  shrink: true
                }}
                disabled
              ></TextField>
            </FormControl>
          </div>
        </div>
        <div className="row top-buffer">
          <div className="col-md-4">
            <label>PIN</label>
          </div>
          <div className="col-md-8">
            <FormControl fullWidth>
              <TextField
                variant="standard"
                value={tag.pIN}
                size="small"
                name="pIN"
                InputLabelProps={{
                  shrink: true
                }}
                onChange={e =>
                  onChange({
                    target: { name: e.target.name, value: e.target.value }
                  })
                }
              ></TextField>
            </FormControl>
          </div>
        </div>
        <div className="row top-buffer">
          <div className="col-md-4">
            <label>Status</label>
          </div>

          <div className="col-md-8">
            <FormControl fullWidth>
              <FormControl variant="outlined" fullWidth>
                <Select
                  labelId="demo-simple-select-label"
                  id="statusID"
                  name="statusID"
                  value={tag.statusID ?? ""}
                  label="Distributor"
                  variant="standard"
                  onChange={e =>
                    onChange({
                      target: {
                        name: e.target.name,
                        value: e.target.value
                      }
                    })
                  }
                >
                  {tagStatusList &&
                    tagStatusList.map(l => {
                      return (
                        <MenuItem key={l.Key} value={l.Key}>
                          {l.Value}
                        </MenuItem>
                      );
                    })}
                </Select>
              </FormControl>
            </FormControl>
          </div>
        </div>
      </div>
    </fieldset>
  );
};

export default TagDetail;
