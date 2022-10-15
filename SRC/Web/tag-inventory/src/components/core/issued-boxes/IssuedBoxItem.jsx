import React, { useEffect, useState } from "react";

import ShipmentDetail from "../shipment/ShipmentDetail";
import { connect } from "react-redux";
import PropTypes from "prop-types";
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

const IssuedBoxItem = props => {
  useEffect(() => {}, []);

  return (
    <React.Fragment>
      <div className="content-area-summary top-buffer">
        <div className="row" style={{ marginTop: 0 }}>
          <div className="col-md-6">
            <fieldset className="border p-2 area ">
              <legend class="w-auto spacing float-none">Issued Box</legend>
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
                        value={props.issuedBox.receivedBoxID}
                        size="small"
                        disabled
                        name="purchaseOrder"
                        InputLabelProps={{
                          shrink: true
                        }}
                      ></TextField>
                    </FormControl>
                  </div>
                </div>
                <div className="row top-buffer">
                  <div className="col-md-4">
                    <label>Quantity</label>
                  </div>
                  <div className="col-md-8">
                    <FormControl fullWidth>
                      <TextField
                        variant="standard"
                        value={props.issuedBox.quantity}
                        disabled
                        size="small"
                        name="purchaseOrder"
                        InputLabelProps={{
                          shrink: true
                        }}
                      ></TextField>
                    </FormControl>
                  </div>
                </div>
                <div className="row top-buffer">
                  <div className="col-md-4">
                    <label>Send Date</label>
                  </div>
                  <div className="col-md-8">
                    {/*   <FormControl fullWidth>
                      <TextField
                        variant="standard"
                        value={props.issuedBox.sendDate}
                        size="small"
                        name="sendDate"
                        id="sendDate"
                        InputLabelProps={{
                          shrink: true,
                        }}
                        onChange={(e) =>
                          props.onChange({
                            name: e.target.name,
                            value: e.target.value,
                          })
                        }
                      ></TextField>
                    </FormControl> */}
                    <FormControl fullWidth>
                      <LocalizationProvider dateAdapter={AdapterDateFns}>
                        <DatePicker
                          value={
                            !props.issuedBox.sendDate
                              ? null
                              : props.issuedBox.sendDate
                          }
                          onChange={e => {
                            console.log(e);
                            props.onChange({
                              name: "sendDate",
                              value: e
                            });
                          }}
                          renderInput={params => (
                            <TextField {...params} variant="standard" />
                          )}
                        />
                      </LocalizationProvider>
                    </FormControl>
                  </div>
                </div>
                <div className="row top-buffer">
                  <div className="col-md-4">
                    <label>Received Date</label>
                  </div>
                  <div className="col-md-8">
                    <FormControl fullWidth>
                      <LocalizationProvider dateAdapter={AdapterDateFns}>
                        <DatePicker
                          value={
                            !props.issuedBox.receivedDate
                              ? null
                              : props.issuedBox.receivedDate
                          }
                          onChange={e => {
                            console.log(e);
                            props.onChange({
                              name: "receivedDate",
                              value: e
                            });
                          }}
                          renderInput={params => (
                            <TextField {...params} variant="standard" />
                          )}
                        />
                      </LocalizationProvider>
                    </FormControl>
                  </div>
                </div>
                <div className="row top-buffer">
                  <div className="col-md-4">
                    <label>Distributor</label>
                  </div>

                  <div className="col-md-8">
                    <FormControl fullWidth>
                      {/*       <TextField
                        variant="standard"
                        value={props.issuedBox.distributor}
                        size="small"
                        name="purchaseOrder"
                        disabled
                        InputLabelProps={{
                          shrink: true,
                        }}
                      ></TextField> */}
                      <FormControl variant="outlined" fullWidth>
                        <Select
                          labelId="demo-simple-select-label"
                          id="distributorID"
                          name="distributorID"
                          value={props.distributorID}
                          label="Distributor"
                          variant="standard"
                          onChange={e =>
                            props.onChange({
                              name: e.target.name,
                              value: e.target.value
                            })
                          }
                        >
                          {props.distributors &&
                            props.distributors.map(l => {
                              return (
                                <MenuItem value={l.distributorID}>
                                  {l.distributorName}
                                </MenuItem>
                              );
                            })}
                        </Select>
                      </FormControl>
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
                        value={props.statusID}
                        label="Shipment Status"
                        variant="standard"
                        onChange={e =>
                          props.onChange({
                            name: e.target.name,
                            value: e.target.value
                          })
                        }
                      >
                        {props.lookup.issuedboxstatus &&
                          props.lookup.issuedboxstatus.map(l => {
                            return <MenuItem value={l.Key}>{l.Value}</MenuItem>;
                          })}
                      </Select>
                    </FormControl>
                  </div>
                </div>
              </div>
            </fieldset>
          </div>

          <div className="col-md-6">
            {props.shipment && (
              <ShipmentDetail
                shipment={props.shipment}
                shipmentID={props.shipment.shipmentID}
                readOnly={true}
              ></ShipmentDetail>
            )}
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

PropTypes.IssuedBoxItem = {
  issuedBox: PropTypes.object.isRequired
};

const mapStateToProps = state => {
  return {
    lookup: state.lookup.lookup
  };
};

export default connect(
  mapStateToProps,
  null
)(IssuedBoxItem);
