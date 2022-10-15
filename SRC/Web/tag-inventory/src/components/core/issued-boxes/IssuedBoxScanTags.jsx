import React, { Component } from "react";
import { useState } from "react";
import Table from "../../common/ui-controls/Table";
import BarcodeReader from "react-barcode-reader";
import PropTypes from "prop-types";
import Alert from "@material-ui/core/Alert";
import AlertTitle from "@material-ui/core/AlertTitle";
import Snackbar from "@material-ui/core/Snackbar";
import { connect } from "react-redux";
import { getTags } from "../../../actions/tagActions";
import config from "../../../config.json";

const IssuedBoxScanTags = ({
  columns,
  getTagsAction,
  appendTag,
  scanTags,
  tags
}) => {
  console.log(columns);

  const [state, setState] = useState({
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
        console.log(tag);
        if (tag.issuedBoxID) {
          openAlert(
            `This tag belongs to another issued box = ${tag.issuedBoxID}. Please remove this tag and place it to its issued box`
          );
          return;
        } else if (tag.statusID !== 1) {
          openAlert("This tag is not verified. Please scan verified Tag only");
          return;
        } else {
          const stateClone = { ...state };

          if (
            tags.find(t => t.tagNumber === tagNumber) ||
            scanTags.find(t => t.tagNumber === tagNumber)
          ) {
            openAlert(`${tagNumber} already Scanned`);
            return;
          }

          if (tags.length === config.boxCount) {
            openAlert("The box already meet its capacity");
            return;
          }

          const t = { ...tag };

          appendTag(t);
          setState(stateClone);
        }
      })
      .catch(error => {
        console.log(error);
        alert("Tag does not exists in system. Please import it");
      });
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
            {tags.length > 0 && (
              <Table
                columns={columns}
                data={scanTags}
                searchVisible={false}
              ></Table>
            )}
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

IssuedBoxScanTags.propTypes = {
  issuedBox: PropTypes.object.isRequired
};

const mapDispatchToProps = dispatch => ({
  getTagsAction: url => dispatch(getTags(url))
});

export default connect(
  null,
  mapDispatchToProps
)(IssuedBoxScanTags);
