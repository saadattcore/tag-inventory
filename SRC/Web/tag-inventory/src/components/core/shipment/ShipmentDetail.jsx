import React, { Component } from "react";
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

const ShipmentDetail = ({
  shipment,
  errors,
  readOnly,
  onChangeValue,
  shipmentID,
}) => {
  return (
    <fieldset className="border p-2 area ">
      <legend class="w-auto spacing float-none">
        {shipmentID ? "Shipment" : "General"}
      </legend>
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
            <label>Shipment Name</label>
          </div>
          <div className="col-md-8">
            <FormControl fullWidth>
              <TextField
                variant="standard"
                value={shipment.shipmentName}
                error={errors && errors["shipmentName"]}
                size="small"
                name="shipmentName"
                disabled={readOnly}
                helperText={errors && errors.shipmentName}
                InputLabelProps={{
                  shrink: true,
                }}
                onChange={onChangeValue}
              ></TextField>
            </FormControl>
          </div>
        </div>

        <div className="row top-buffer">
          <div className="col-md-4">
            <label>Purchase Order</label>
          </div>
          <div className="col-md-8">
            <FormControl fullWidth>
              <TextField
                variant="standard"
                value={shipment.purchaseOrder}
                error={errors && errors["purchaseOrder"]}
                size="small"
                name="purchaseOrder"
                disabled={readOnly}
                helperText={errors && errors.purchaseOrder}
                InputLabelProps={{
                  shrink: true,
                }}
                onChange={onChangeValue}
              ></TextField>
            </FormControl>
          </div>
        </div>

        <div className="row top-buffer">
          <div className="col-md-4">
            <label>Order Date</label>
          </div>
          <div className="col-md-8">
            <FormControl fullWidth>
              <LocalizationProvider dateAdapter={AdapterDateFns}>
                <DatePicker
                  value={shipment.orderDate}
                  onChange={(capture) =>
                    onChangeValue.call(this, {
                      target: { name: "orderDate", value: capture },
                    })
                  }
                  renderInput={(params) => (
                    <TextField
                      {...params}
                      error={errors && errors["orderDate"]}
                      helperText={errors && errors.orderDate}
                      variant="standard"
                      disabled={readOnly}
                    />
                  )}
                />
              </LocalizationProvider>
            </FormControl>
          </div>
        </div>
        <div className="row top-buffer">
          <div className="col-md-4">
            <label>Shipment Date</label>
          </div>
          <div className="col-md-8">
            <FormControl fullWidth>
              <LocalizationProvider dateAdapter={AdapterDateFns}>
                <DatePicker
                  emptyLabel="custom label"
                  value={shipment.shipmentDate}
                  onChange={(capture) =>
                    onChangeValue.call(this, {
                      target: { name: "shipmentDate", value: capture },
                    })
                  }
                  renderInput={(params) => (
                    <TextField
                      {...params}
                      error={errors && errors["shipmentDate"]}
                      helperText={errors && errors.shipmentDate}
                      variant="standard"
                      disabled={readOnly}
                    />
                  )}
                />
              </LocalizationProvider>
            </FormControl>
          </div>
        </div>
        {shipment.shipmentID !== -1 && (
          <div className="row top-buffer">
            <div className="col-md-4">
              <label>Delivery Date</label>
            </div>
            <div className="col-md-8">
              <FormControl fullWidth>
                <LocalizationProvider dateAdapter={AdapterDateFns}>
                  <DatePicker
                    size="small"
                    value={shipment.deliveryDate}
                    onChange={(capture) =>
                      onChangeValue.call(this, {
                        target: { name: "deliveryDate", value: capture },
                      })
                    }
                    renderInput={(params) => (
                      <TextField
                        {...params}
                        error={errors && errors["deliveryDate"]}
                        helperText={errors && errors.deliveryDate}
                        variant="standard"
                        disabled={readOnly}
                      />
                    )}
                  />
                </LocalizationProvider>
              </FormControl>
            </div>
          </div>
        )}
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
                value={shipment.statusID ?? ""}
                label="Shipment Status"
                onChange={onChangeValue}
                variant="standard"
                disabled={readOnly}
              >
                <MenuItem value={1}>Ordered</MenuItem>
                <MenuItem value={2}>Delivered</MenuItem>
              </Select>
            </FormControl>
          </div>
        </div>
      </div>
    </fieldset>
  );
};

export default ShipmentDetail;
