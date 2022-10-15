import React, { useState, useEffect, useRef } from "react";
import PropTypes from "prop-types";
import config from "../../../config.json";
import Table from "../../common/ui-controls/Table";
import IssuedBoxScanTagActions from "../../common/reuseable/IssuedBoxScanTagActions";

const IssuedBoxTags = (props) => {
  useEffect(() => {}, []);

  return (
    <React.Fragment>
      <div className="">
        <div className="actions-import-box-no-margin top-buffer">
          <IssuedBoxScanTagActions
            disableVerifyScanTag={
              props.issuedBox && props.issuedBox.tags.length == config.boxCount
            }
            disableAssignTags={
              props.issuedBox && props.issuedBox.tags.length < config.boxCount
            }
            onLinkClick={props.onLinkClick}
            tagsCount={props.issuedBox && props.issuedBox.tags.length}
          ></IssuedBoxScanTagActions>
        </div>

        <div className="row top-buffer">
          <div className="list-container">
            <Table
              columns={props.columns}
              data={props.issuedBox.tags}
              searchVisible={false}
            ></Table>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

PropTypes.IssuedBoxTags = {
  issuedBox: PropTypes.object.isRequired,
};

export default IssuedBoxTags;
