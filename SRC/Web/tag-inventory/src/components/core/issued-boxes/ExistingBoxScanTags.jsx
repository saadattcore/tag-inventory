import React, { Component } from "react";
import { useState } from "react";
import Table from "../../common/ui-controls/Table";
import Actions from "../../common/reuseable/Actions";
import BarcodeReader from "react-barcode-reader";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import Alert from "@material-ui/core/Alert";
import AlertTitle from "@material-ui/core/AlertTitle";
import Snackbar from "@material-ui/core/Snackbar";
import { getTags } from "../../../actions/tagActions";
import { appendScanTagExisting } from "../../../actions/issuedBoxActions";

const ExistingBoxScanTags = ({
  columns,
  getTagsAction,
  appendScanTagExistingAction,
  issuedBox
}) => {
  console.log(columns);

  const [state, setState] = useState({
    scanTags: [],
    error: "",
    showError: false
  });

  const handleScan = tagNumber => {
    const url = `${
      process.env.REACT_APP_API_URL
    }tag/list?pageSize=${1}&pageNumber=${1}&tagNumber=${tagNumber}`;
    getTagsAction(url)
      .then(response => {
        console.log(response);
        const tag = response[0];
        if (tag.issuedBoxID && tag.issuedBoxID !== issuedBox.issuedBoxID) {
          alert(
            `This tag belongs to another issued box = ${tag.issuedBoxID}. Please remove this tag and place it to its issued box`
          );
        } else if (tag.statusID !== 1) {
          alert("This tag is not verified. Please scan verified Tag only");
        } else {
          const stateClone = { ...state };

          if (stateClone.scanTags.find(t => t.tagNumber === tagNumber)) {
            openAlert(`${tagNumber} already Scanned`);
            return;
          }

          if (issuedBox.tags.find(t => t.tagNumber === tagNumber)) {
            openAlert(`${tagNumber} already added to issued box`);
            return;
          }

          const t = { ...response[0] };
          t.issuedBoxID = issuedBox.issuedBoxID;

          appendScanTagExistingAction(t).then(response => {
            console.log(response);

            stateClone.scanTags.push({ ...response });
            setState(stateClone);
            //setScanTags(arr => [...arr, response]);
          });
        }
      })
      .catch(error => console.log(error));
  };

  const openAlert = content => {
    const cloneState = { ...state };
    cloneState.showError = true;
    cloneState.error = content;
    setState(cloneState);
  };

  const handleAlertClose = () => {
    const cloneState = { ...state };
    cloneState.showError = false;
    setState(cloneState);
  };

  return (
    <React.Fragment>
      <BarcodeReader onError={null} onScan={handleScan} />
      <Snackbar
        open={state.showError}
        autoHideDuration={4000}
        onClose={handleAlertClose}
      >
        <Alert severity="error">
          <AlertTitle>Error</AlertTitle>
          <strong>{state.error}</strong>
        </Alert>
      </Snackbar>
      <div className="content-area-main">
        <div className="content-title">
          <h4>Scan Verified Tags</h4>
        </div>
        <div className="row top-buffer">
          <div className="list-container">
            {state.scanTags.length > 0 && (
              <Table
                columns={columns}
                data={state.scanTags}
                searchVisible={false}
              ></Table>
            )}
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

ExistingBoxScanTags.propTypes = {
  issuedBox: PropTypes.object.isRequired
};

const mapStateToProps = state => {
  return {
    issuedBox: state.issuedBox.issuedBox
  };
};

const mapDispatchToProps = dispatch => ({
  getTagsAction: url => dispatch(getTags(url)),
  appendScanTagExistingAction: scanTag =>
    dispatch(appendScanTagExisting(scanTag))
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(ExistingBoxScanTags);
