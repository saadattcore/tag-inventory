import React, { useState, useEffect } from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import DatePicker from "@material-ui/lab/DatePicker";
import AdapterDateFns from "@material-ui/lab/AdapterDateFns";
import LocalizationProvider from "@material-ui/lab/LocalizationProvider";
import moment from "moment";
import Actions from "../../common/reuseable/Actions";
import { TextField, FormControl } from "@material-ui/core";
import { updateIssuedBoxesStatus } from "../../../actions/issuedBoxActions";

const IssuedBoxSendToPress = props => {
  const [state, setState] = useState({
    sendDate: null,
    errors: {},
    isFormValid: true
  });

  useEffect(() => {
    const stateClone = { ...state };
    stateClone.sendDate = moment(new Date()).format("MM-DD-YYYY");
    if (props.issuedBoxes.length === 0) {
      alert("Please select issued boxes for press");
      return;
    }
    console.log(props.issuedBoxes);
    setState(stateClone);
  }, []);

  const onChangeValue = ({ target }) => {
    const { name, value } = target;

    const stateClone = { ...state };

    if (!value) {
      stateClone.errors = { sendDate: "Send date is required" };
    } else {
      delete stateClone.errors["sendDate"];
      stateClone.sendDate = moment(value).format("MM-DD-YYYY");
    }

    setState(stateClone);
  };

  const handleClick = ({ target }) => {
    const url = `${process.env.REACT_APP_API_URL}issued-box/update-boxes-status`;
    const issuedBoxList = [...props.issuedBoxes.filter(b => b.statusID === 3)];
    issuedBoxList.forEach(b => {
      b.statusID = 4;
      b.sendDate = state.sendDate;
    });

    switch (target.action) {
      case "save":
        props.updateIssuedBoxesStatusAction(url, issuedBoxList).then(r => {
          alert("Sucessfully updated status");
        });
        break;
      case "saveclose":
        props.updateIssuedBoxesStatusAction(url, issuedBoxList).then(r => {
          props.history.push("/issued-box");
        });
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
          isFormValid={state.isFormValid}
          onLinkClick={handleClick}
        ></Actions>
      </div>

      <div style={{ paddingTop: 40, paddingLeft: 40 }}>
        <div className="row">
          <div className="col-md-4">
            <fieldset className="border p-2 area ">
              <legend class="w-auto spacing float-none">
                {"Send To Press"}
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
                          value={state.sendDate}
                          onChange={capture =>
                            onChangeValue.call(this, {
                              target: { name: "sendDate", value: capture }
                            })
                          }
                          renderInput={params => (
                            <TextField
                              {...params}
                              error={state.errors && state.errors["sendDate"]}
                              helperText={state.errors && state.errors.sendDate}
                              variant="standard"
                            />
                          )}
                        />
                      </LocalizationProvider>
                    </FormControl>
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

const mapDispatchToProps = dispatch => ({
  updateIssuedBoxesStatusAction: (url, boxList) =>
    dispatch(updateIssuedBoxesStatus(url, boxList))
});

const mapStateToProps = state => {
  return {
    issuedBoxes: state.issuedBox.issuedBoxes
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(IssuedBoxSendToPress);
