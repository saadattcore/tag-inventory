import React, { Component } from "react";
import CircularProgress from "@material-ui/core/CircularProgress";
import TextField from "@material-ui/core/TextField";
import Autocomplete from "@material-ui/core/Autocomplete";
import { Select, FormControl, MenuItem, InputLabel } from "@material-ui/core";
import http, { HttpClient } from "../../../services/HttpModule";
import ReceivedBoxes from "./ReceivedBoxes";
import Actions from "../../common/reuseable/Actions";
import config from "../../../config.json";
import { Link } from "react-router-dom";
import Table from "../../common/ui-controls/Table";
import queryString from "query-string";
import { renderAlert } from "../../common/ui-controls/withAlert";

class ImportBox extends Component {
  constructor(props) {
    super(props);
    this.receivedBoxType = [{ id: 1, label: "New" }, { id: 2, label: "Spare" }];
    this.initAutoCompleteLabel = "Select Shipment";
  }

  columns = [
    {
      path: "receivedBoxID",
      title: "ReceivedBoxID",
      key: 1,
      content: id => {
        const navigateUrl = `/received-box/${id}`;
        return <Link to={navigateUrl}>{id}</Link>;
      }
    },
    { path: "startTag", title: "StartFrom", key: 2 },
    { path: "endTag", title: "StartTo", key: 3 },
    { path: "quantity", title: "Quantity", key: 4 }
  ];

  state = {
    updFiles: [],
    isBtnDisable: false,
    isLoading: false,
    txtShipments: [],
    selectShipment: { id: -1, label: "" },
    selectBoxType: 1,
    importedReceivedBoxes: []
  };

  async getShipments(shipmentSeacrhKey) {
    let uri = "shipment/list?pageSize=100&pageNumber=1";

    console.log("getshiments");

    if (shipmentSeacrhKey) {
      uri = uri + "&purchaseOrder=" + shipmentSeacrhKey;
    }
    try {
      const { data } = await http.get(process.env.REACT_APP_API_URL + uri);
      let shipments = [];

      shipments = data.data.map(shipment => {
        return { label: shipment.purchaseOrder, id: shipment.shipmentID };
        console.log(data);
      });

      return shipments;
    } catch (ex) {
      console.log(ex);
      return [{}];
    }
  }

  async componentDidMount() {
    console.log("componentDidMount");
    console.log(this.props);
    const stateClone = { ...this.state };
    const shipments = await this.getShipments();

    console.log(shipments);
    console.log(typeof shipments);

    stateClone.txtShipments = shipments;

    if (
      this.props.location.search &&
      Object.keys(queryString.parse(this.props.location.search)).length > 0
    ) {
      const shipmentID = queryString.parse(this.props.location.search)
        .shipmentID;

      if (shipments.length > 0) {
        this.initAutoCompleteLabel = shipments.filter(
          t => t.id === parseInt(shipmentID)
        )[0].label;

        this.state.selectShipment.id = shipmentID;
      }
    }

    this.setState(stateClone);
  }

  async handleChange({ target }) {
    const stateClone = { ...this.state };
    let { name, value } = target;

    if (name === "txtShipments" || (name === "selectShipment") & !value) {
      console.log(name);
      console.log(value);
      value = await this.getShipments(value);

      if (name === "selectShipment") {
        name = "txtShipments";
      }

      console.log(value);
    }
    if (name === "updFiles") value = [...target.files];

    stateClone[name] = value;
    this.setState(stateClone);
  }

  handleSubmit(e) {
    e.preventDefault();

    const spinnerStartState = { ...this.state };

    if (
      spinnerStartState.updFiles.length === 0 ||
      spinnerStartState.selectShipment.id === -1
    ) {
      // alert("Upload files and shipment cannot be empty");

      this.displayAlert("Upload files and shipment cannot be empty", "error");

      return;
    }

    spinnerStartState.isLoading = true;
    spinnerStartState.isBtnDisable = true;

    const formData = new FormData();

    for (let index = 0; index < spinnerStartState.updFiles.length; index++) {
      const fileObject = spinnerStartState.updFiles[index];
      console.log(fileObject);
      formData.append(fileObject.name, fileObject);
    }

    formData.append("shipmentID", this.state.selectShipment.id);
    formData.append("shipmentType", this.state.selectBoxType);

    const headers = {
      "content-type": "multipart/form-data",
      "Access-Control-Allow-Origin": "*"
    };

    http
      .post(`${process.env.REACT_APP_API_URL}received-box/import`, formData, {
        headers: headers
      })
      .then(response => {
        console.log(response.data);

        const spinnerStopState = { ...this.state };
        spinnerStopState.importedReceivedBoxes = response.data;
        spinnerStopState.isLoading = false;
        spinnerStopState.isBtnDisable = false;
        this.setState(spinnerStopState);
      })
      .catch(error => {
        console.log(error.response);

        if (error.response && error.response.data) {
          this.displayAlert(error.response.data, "error").then(r => {
            const spinnerStopState = { ...this.state };
            spinnerStopState.isLoading = false;
            spinnerStopState.isBtnDisable = false;
            this.setState(spinnerStopState);
          });
        } else {
          alert("Please refresh the browser and try again");
        }
      });

    this.setState(spinnerStartState);
  }

  handleClick({ target }) {
    const { action } = target;
    switch (action) {
      case "back":
        this.props.history.goBack();
        break;
    }
  }

  displayAlert(content, type) {
    return new Promise((resolve, reject) => {
      const stateClone = { ...this.state };
      stateClone.showAlert = renderAlert.bind(null, type, content, true, e => {
        stateClone.showAlert = null;
        this.setState(stateClone);
        resolve("fulfilled");
      });

      this.setState(stateClone);
    });
  }

  render() {
    return (
      <React.Fragment>
        <div className="actions">
          <Actions
            isFormValid={this.state.isFormValid}
            onLinkClick={this.handleClick.bind(this)}
          ></Actions>
        </div>
        <div className="content-title">
          <h5>New Received Box</h5>
        </div>
        {this.state.showAlert && this.state.showAlert()}
        <div className="content-area">
          <div className="row">
            <div className="col-md-4">
              <fieldset className="border p-2 area ">
                <legend class="w-auto spacing float-none">Import Box</legend>
                <div className="legend-content">
                  <div className="row" id="importBoxForm">
                    <div className="col-3">Shipment</div>
                    <div className="col-8">
                      <Autocomplete
                        disablePortal
                        id="selectShipment"
                        autoComplete={true}
                        options={this.state.txtShipments}
                        getOptionLabel={option => option.label}
                        onChange={(event, value) =>
                          this.handleChange({
                            target: { name: "selectShipment", value: value }
                          })
                        }
                        renderInput={params => (
                          <TextField
                            {...params}
                            id="txtShipments"
                            label={this.initAutoCompleteLabel}
                            variant="standard"
                            onChange={e =>
                              this.handleChange({
                                target: {
                                  name: "txtShipments",
                                  value: e.target.value
                                }
                              })
                            }
                          />
                        )}
                      />
                    </div>
                  </div>
                  <div className="row top-buffer">
                    <div className="col-3">File Location</div>
                    <div className="col-8">
                      <input
                        type="file"
                        name="updFiles"
                        id="updFiles"
                        multiple
                        className="form-control"
                        onChange={this.handleChange.bind(this)}
                      ></input>
                    </div>
                  </div>
                  <div className="row top-buffer">
                    <div className="col-3">Type</div>
                    <div className="col-8">
                      <FormControl variant="outlined" fullWidth>
                        <Select
                          id="selectBoxType"
                          name="selectBoxType"
                          label="Select box type"
                          onChange={this.handleChange.bind(this)}
                          value={this.state.selectBoxType}
                          variant="standard"
                        >
                          <MenuItem value={1}>New</MenuItem>
                          <MenuItem value={2}>Spare</MenuItem>
                        </Select>
                      </FormControl>
                    </div>
                  </div>
                  <div className="row top-buffer">
                    <div className="col-3"></div>
                    <div className="col-8">
                      <button
                        className="btn btn-primary"
                        disabled={this.state.isBtnDisable}
                        onClick={this.handleSubmit.bind(this)}
                      >
                        Import Boxes
                      </button>
                    </div>
                    <div className="col-1">
                      {this.state.isLoading && (
                        <CircularProgress
                          color="secondary"
                          size={20}
                        ></CircularProgress>
                      )}
                    </div>
                  </div>
                </div>
              </fieldset>
            </div>
          </div>
          {this.state.importedReceivedBoxes.length > 0 && (
            <div className="import-received-boxes">
              <Table
                columns={this.columns}
                data={this.state.importedReceivedBoxes}
                searchVisible={false}
              ></Table>
            </div>
          )}
        </div>
      </React.Fragment>
    );
  }
}

export default ImportBox;
