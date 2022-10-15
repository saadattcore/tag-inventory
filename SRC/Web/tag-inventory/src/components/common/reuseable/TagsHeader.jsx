import React from "react";

const TagsHeader = props => {

    return (
        <React.Fragment>
            <div className="actions">
        <Actions onLinkClick={handleClick}></Actions>
      </div>
      <div className="content-area-main">
        <div className="content-title">
          <h3>{receivedBoxID}</h3>
        </div>
        <div className="shipment-detail-summary">
          <Link>Summary</Link> &nbsp; | &nbsp;
          <span>Received Tags</span>
        </div>
        <div className="actions-import-box i-size">
          <ImportScanActions
            actionFormType="scanTags"
            navigateURL={
              receivedBoxID
                ? `/received-box/${receivedBoxID}/scan-tags`
                : "/tag/scan"
            }
          ></ImportScanActions>
        </div>
        </React.Fragment>

    )

};

export default TagsHeader;
