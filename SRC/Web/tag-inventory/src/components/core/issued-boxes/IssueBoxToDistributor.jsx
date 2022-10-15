import React, { useState, useEffect } from "react";
import { getDistributors } from "../../../actions/lookupActions";
import { connect } from "react-redux";
import PropertyTypes from "prop-types";
import DatePicker from "@material-ui/lab/DatePicker";
import AdapterDateFns from "@material-ui/lab/AdapterDateFns";
import LocalizationProvider from "@material-ui/lab/LocalizationProvider";
import moment from "moment";
import Actions from "../../common/reuseable/Actions";
import {
  updateIssuedBoxesStatus,
  downloadSerialList
} from "../../../actions/issuedBoxActions";
import {
  TextField,
  FormControl,
  Select,
  MenuItem,
  InputLabel,
  FormHelperText
} from "@material-ui/core";

const IssueBoxToDistributor = props => {
  const [state, setState] = useState({
    issuedDate: null,
    distributors: [],
    distributorID: "",
    selectedDistType: 0,
    errors: {}
  });

  useEffect(() => {
    const stateClone = { ...state };
    const url = `${process.env.REACT_APP_API_URL}lookup/get-distributors`;
    stateClone.issuedDate = moment(new Date()).format("MM-DD-YYYY");
    props.getDistributorsAction(url).then(response => {
      console.log(response);
      stateClone.errors["distributor"] = true;
      setState(stateClone);
    });
  }, []);

  const onChangeValue = ({ value, name }) => {
    const stateClone = { ...state };

    switch (name) {
      case "distributorType":
        value = parseInt(value);

        if (value === 0) {
          stateClone.errors["distributor"] = "Distributor is required";
        }

        stateClone.selectedDistType = value;
        stateClone.distributorID = 0;

        stateClone.distributors = [
          ...props.distributors.filter(d => d.distributorTypeId === value)
        ];

        if (stateClone.distributors.length > 0) {
          stateClone.distributors.splice(0, 0, {
            distributorName: "SELECT",
            distributorID: 0
          });
        }
        break;

      case "distributor":
        value = parseInt(value);
        stateClone.distributorID = value;

        if (value > 0) {
          delete stateClone.errors["distributor"];
        } else stateClone.errors["distributor"] = true;

        break;

      case "issuedDate":
        if (!value) {
          stateClone.errors["issuedDate"] = "Issued date is required";
        } else {
          delete stateClone.errors["issuedDate"];
          stateClone.issuedDate = moment(value).format("MM-DD-YYYY");
        }
        break;
    }

    setState(stateClone);
  };

  const linkBoxToDistributor = (distributorID, postSaveAction) => {
    const url = `${process.env.REACT_APP_API_URL}issued-box/update-boxes-status`;

    const issuedBoxList = [
      ...props.issuedBoxes
        .filter(b => b.statusID === 5)
        .map(b => {
          const ib = {};
          ib.issuedBoxID = b.issuedBoxID;
          ib.distributorID = distributorID;
          ib.statusID = 7;
          ib.issuedDate = state.issuedDate;
          return ib;
        })
    ];

    console.log(issuedBoxList);

    props.updateIssuedBoxesStatusAction(url, issuedBoxList).then(r => {
      let issuedBoxIDList = props.issuedBoxes
        .filter(b => b.selected)
        .map(b => b.issuedBoxID)
        .join(",");
      console.log(issuedBoxIDList);

      let baseUrl = `${process.env.REACT_APP_API_URL}issued-box/serial-list?issuedBoxIDList=${issuedBoxIDList}`;
      console.log(baseUrl);
      props.downloadSerialListAction(baseUrl).then(response => {
        if (postSaveAction === "saveclose") {
          props.history.push("/issued-box");
        }
      });
      //alert("Sucessfully updated status");
    });
  };

  const handleClick = ({ target }) => {
    switch (target.action) {
      case "save":
        linkBoxToDistributor(state.distributorID, "save");
        break;
      case "saveclose":
        linkBoxToDistributor(state.distributorID, "saveclose");
        break;
      case "back":
        props.history.push("/issued-box");
        break;
    }
  };

  return (
    <React.Fragment>
      <div className="actions">
        <Actions
          isFormValid={Object.keys(state.errors).length === 0}
          onLinkClick={handleClick}
        ></Actions>
      </div>

      <div style={{ paddingTop: 40, paddingLeft: 40 }}>
        <div className="row">
          <div className="col-md-4">
            <fieldset className="border p-2 area ">
              <legend class="w-auto spacing float-none">
                {"Issue Boxes To Distributor"}
              </legend>
              <div
                style={{
                  paddingTop: 5,
                  paddingRight: 25,
                  paddingBottom: 35,
                  paddingLeft: 25
                }}
              >
                <div className="row top-buffer">
                  <div className="col-md-3 label-container">
                    <label>Send Date</label>
                  </div>
                  <div className="col-md-8">
                    <FormControl fullWidth>
                      <LocalizationProvider dateAdapter={AdapterDateFns}>
                        <DatePicker
                          value={state.issuedDate}
                          onChange={capture => {
                            onChangeValue.call(this, {
                              value: capture,
                              name: "issuedDate"
                            });
                          }}
                          renderInput={params => (
                            <TextField
                              {...params}
                              error={state.errors && state.errors["issuedDate"]}
                              helperText={
                                state.errors && state.errors.issuedDate
                              }
                              variant="standard"
                            />
                          )}
                        />
                      </LocalizationProvider>
                    </FormControl>
                  </div>
                </div>
                <div className="row top-buffer">
                  <div className="col-md-3 label-container">
                    <label>Distributor Type</label>
                  </div>
                  <div className="col-md-8">
                    <FormControl variant="outlined" fullWidth>
                      <Select
                        labelId="demo-simple-select-label"
                        id="statusID"
                        name="statusID"
                        value={state.selectedDistType}
                        label="Shipment Status"
                        displayEmpty={true}
                        onChange={arg => {
                          onChangeValue.call(this, {
                            value: arg.target.value,
                            name: "distributorType"
                          });
                        }}
                        variant="standard"
                      >
                        <MenuItem value={0}>SELECT</MenuItem>

                        {props.distributorTypes.map(t => {
                          return (
                            <MenuItem value={t.distributorTypeID}>
                              {t.distributorTypeName}
                            </MenuItem>
                          );
                        })}
                      </Select>
                    </FormControl>

                    {/*   <select
                      onChange={arg => {
                        onChangeValue.call(this, {
                          value: arg.target.value,
                          type: "DistributorType"
                        });
                      }}
                    >
                      <option value={0}>SELECT</option>
                      {props.distributorTypes.map(t => {
                        return (
                          <option value={t.distributorTypeID}>
                            {t.distributorTypeName}
                          </option>
                        );
                      })}
                    </select> */}
                  </div>
                </div>
                <div className="row top-buffer">
                  <div className="col-md-3 label-container">
                    <label>Distributor</label>
                  </div>
                  <div className="col-md-8">
                    <FormControl required variant="outlined" fullWidth>
                      <Select
                        labelId="demo-simple-select-required-label"
                        id="demo-simple-select-required"
                        name="statusID"
                        value={state.distributorID}
                        label="Distributor *"
                        onChange={arg => {
                          onChangeValue.call(this, {
                            value: arg.target.value,
                            name: "distributor"
                          });
                        }}
                        variant="standard"
                      >
                        {state.distributors.map(t => {
                          return (
                            <MenuItem value={t.distributorID}>
                              {t.distributorName}
                            </MenuItem>
                          );
                        })}
                      </Select>
                      {state.errors.distributor && (
                        <FormHelperText>
                          {"Distributor is requires"}
                        </FormHelperText>
                      )}
                    </FormControl>
                    {/*      <select
                      onChange={arg =>
                        onChangeValue.call(this, {
                          value: arg.target.value,
                          type: "Distributor"
                        })
                      }
                    >
                      {state.distributors.map(d => {
                        return (
                          <option value={d.distributorID}>
                            {d.distributorName}
                          </option>
                        );
                      })}
                      {console.log(state.distributors)}
                    </select> */}
                  </div>
                </div>
              </div>
            </fieldset>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

const mapStateToProps = state => {
  return {
    distributors: state.lookup.distributors,
    distributorTypes: state.lookup.distributorTypes,
    issuedBoxes: state.issuedBox.issuedBoxes
  };
};

const mapDispatchToProps = dispatch => ({
  getDistributorsAction: url => dispatch(getDistributors(url)),
  updateIssuedBoxesStatusAction: (url, boxList) =>
    dispatch(updateIssuedBoxesStatus(url, boxList)),
  downloadSerialListAction: url => dispatch(downloadSerialList(url))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(IssueBoxToDistributor);
